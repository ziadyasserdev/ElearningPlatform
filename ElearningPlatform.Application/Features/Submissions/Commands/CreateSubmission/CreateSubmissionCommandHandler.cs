using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Contracts.Services;
using ElearningPlatform.Domain.Enums;
using ElearningPlatform.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Commands.CreateSubmission
{
    public class CreateSubmissionCommandHandler : IRequestHandler<CreateSubmissionCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IFileService fileService;

        public CreateSubmissionCommandHandler(IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.fileService = fileService;
        }
        public async Task<Result<int>> Handle(
    CreateSubmissionCommand request,
    CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<int>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

            var studentId = currentUserService.UserId;

            var assignment = await unitOfWork.Assignments
                .Query()
                .FirstOrDefaultAsync(a =>
                    a.Id == request.AssignmentId &&
                    !a.IsDeleted,
                    cancellationToken);

            if (assignment is null)
            {
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    "Assignment not found.");
            }

            var isEnrolled = await unitOfWork.Enrollments
                .Query()
                .AnyAsync(e =>
                    e.CourseId == assignment.CourseId &&
                    e.StudentId == studentId &&
                    e.Status == EnrollmentStatus.Active,
                    cancellationToken);

            if (!isEnrolled)
            {
                return Result<int>.Failure(
                    ResultStatus.Forbidden,
                    "You are not enrolled in this course.");
            }

            if (!assignment.IsPublished)
            {
                return Result<int>.Failure(
                    ResultStatus.Failure,
                    "Assignment is not published.");
            }

            if (assignment.IsClosed)
            {
                return Result<int>.Failure(
                    ResultStatus.Failure,
                    "Assignment is closed.");
            }

            if (assignment.OpenAt > DateTime.UtcNow)
            {
                return Result<int>.Failure(
                    ResultStatus.Failure,
                    "Assignment is not open yet.");
            }

            if (assignment.DueDate < DateTime.UtcNow &&
                !assignment.AllowLateSubmission)
            {
                return Result<int>.Failure(
                    ResultStatus.Failure,
                    "Submission deadline has passed.");
            }

            var alreadySubmitted = await unitOfWork.Submissions
                .Query()
                .AnyAsync(s =>
                    s.AssignmentId == assignment.Id &&
                    s.StudentId == studentId &&
                    !s.IsDeleted,
                    cancellationToken);

            if (alreadySubmitted)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "You have already submitted this assignment.");
            }

            var uploadResult = await fileService.UploadFileAsync(request.File);

            if (!uploadResult.IsSuccess)
            {
                return Result<int>.Failure(
                    uploadResult.Status,
                    uploadResult.Error);
            }

            try
            {
                var isLate = DateTime.UtcNow > assignment.DueDate;

                var submission = new Submission
                {
                    AssignmentId = assignment.Id,
                    StudentId = studentId!,
                    FileName = request.File.FileName,
                    FileUrl = uploadResult.Value!,
                    ContentType = request.File.ContentType,
                    FileSize = request.File.Length,
                    Comment = request.Comment,
                    SubmittedAt = DateTime.UtcNow,
                    IsLate = isLate,
                    Status = isLate
                        ? SubmissionStatus.Late
                        : SubmissionStatus.Submitted,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = currentUserService.UserName
                };

                await unitOfWork.Submissions.AddAsync(submission);

                await unitOfWork.SaveAsync();

                return Result<int>.Success(submission.Id);
            }
            catch
            {
                fileService.Remove(uploadResult.Value!);

                throw;
            }
        }
    }
}
