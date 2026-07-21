using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Contracts.Services;
using ElearningPlatform.Application.Features.Submissions.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Queries.DownloadSubmission
{
    public class DownloadSubmissionQueryHandler
    : IRequestHandler<
        DownloadSubmissionQuery,
        Result<DownloadSubmissionResult>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IFileService fileService;

        public DownloadSubmissionQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.fileService = fileService;
        }

        public async Task<Result<DownloadSubmissionResult>> Handle(
            DownloadSubmissionQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<DownloadSubmissionResult>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

            var userId = currentUserService.UserId;

            var submission = await unitOfWork.Submissions
                .Query()
                .AsNoTracking()
                .Where(s =>
                    s.Id == request.SubmissionId &&
                    !s.IsDeleted)
                .Select(s => new
                {
                    s.Id,
                    s.StudentId,
                    s.FileUrl,
                    s.FileName,
                    s.ContentType,

                    InstructorUserId =
                        s.Assignment.Course.Instructor.UserId
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (submission is null)
            {
                return Result<DownloadSubmissionResult>.Failure(
                    ResultStatus.NotFound,
                    "Submission not found.");
            }

          
            var isStudentOwner =
                submission.StudentId == userId;

          
            var isAssignmentInstructor =
                submission.InstructorUserId == userId;

            if (!isStudentOwner && !isAssignmentInstructor)
            {
                return Result<DownloadSubmissionResult>.Failure(
                    ResultStatus.Forbidden,
                    "You are not authorized to download this submission.");
            }

            var fileResult =
                await fileService.DownloadFileAsync(
                    submission.FileUrl);

            if (!fileResult.IsSuccess)
            {
                return Result<DownloadSubmissionResult>.Failure(
                    fileResult.Status,
                    fileResult.Error);
            }

            var result = new DownloadSubmissionResult
            {
                FileBytes = fileResult.Value!,
                FileName = submission.FileName,
                ContentType = submission.ContentType
            };

            return Result<DownloadSubmissionResult>.Success(result);
        }
    }
}
