using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Contracts.Services;
using ElearningPlatform.Application.Features.Submissions.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Queries.DownloadAllSubmissions
{
    public class DownloadAllSubmissionsQueryHandler
      : IRequestHandler<
          DownloadAllSubmissionsQuery,
          Result<DownloadAllSubmissionsResult>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IFileService fileService;

        public DownloadAllSubmissionsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.fileService = fileService;
        }

        public async Task<Result<DownloadAllSubmissionsResult>> Handle(
            DownloadAllSubmissionsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<DownloadAllSubmissionsResult>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

            var userId = currentUserService.UserId;

           
            var assignment = await unitOfWork.Assignments
                .Query()
                .AsNoTracking()
                .Where(a =>
                    a.Id == request.AssignmentId &&
                    !a.IsDeleted &&
                    a.Course.Instructor.UserId == userId)
                .Select(a => new
                {
                    a.Id,
                    a.Title
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (assignment is null)
            {
                return Result<DownloadAllSubmissionsResult>.Failure(
                    ResultStatus.NotFound,
                    "Assignment not found.");
            }
  
            var submissions = await unitOfWork.Submissions
                .Query()
                .AsNoTracking()
                .Where(s =>
                    s.AssignmentId == assignment.Id &&
                    !s.IsDeleted)
                .Select(s => new
                {
                    s.Id,
                    s.StudentId,
                    s.FileUrl
                })
                .ToListAsync(cancellationToken);

            if (submissions.Count == 0)
            {
                return Result<DownloadAllSubmissionsResult>.Failure(
                    ResultStatus.NotFound,
                    "No submissions found for this assignment.");
            }

            using var memoryStream = new MemoryStream();

            using (var archive = new ZipArchive(
                memoryStream,
                ZipArchiveMode.Create,
                leaveOpen: true))
            {
                foreach (var submission in submissions)
                {
                    var fileResult =
                        await fileService.DownloadFileAsync(
                            submission.FileUrl);

                 
                    if (!fileResult.IsSuccess ||
                        fileResult.Value is null)
                    {
                        continue;
                    }

                    var extension =
                        Path.GetExtension(submission.FileUrl);

                    var fileName =
                        $"Student_{submission.StudentId}_Submission_{submission.Id}{extension}";

                    var zipEntry =
                        archive.CreateEntry(
                            fileName,
                            CompressionLevel.Fastest);

                    await using var entryStream =
                        zipEntry.Open();

                    await entryStream.WriteAsync(
                        fileResult.Value,
                        cancellationToken);
                }
            }

            var zipBytes = memoryStream.ToArray();

            if (zipBytes.Length == 0)
            {
                return Result<DownloadAllSubmissionsResult>.Failure(
                    ResultStatus.Failure,
                    "No valid submission files were found.");
            }

            var safeAssignmentTitle =
                string.Join(
                    "_",
                    assignment.Title
                        .Split(Path.GetInvalidFileNameChars()));

            return Result<DownloadAllSubmissionsResult>.Success(
                new DownloadAllSubmissionsResult
                {
                    FileBytes = zipBytes,

                    FileName =
                        $"{safeAssignmentTitle}_Submissions.zip",

                    ContentType = "application/zip"
                });
        }
    }
}
