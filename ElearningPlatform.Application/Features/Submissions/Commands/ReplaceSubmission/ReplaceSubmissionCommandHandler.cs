using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Contracts.Services;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Commands.ReplaceSubmission
{
    public class ReplaceSubmissionCommandHandler
         : IRequestHandler<ReplaceSubmissionCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IFileService fileService;

        public ReplaceSubmissionCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.fileService = fileService;
        }

        public async Task<Result<string>> Handle(
            ReplaceSubmissionCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

            var studentId = currentUserService.UserId;

            var submission = await unitOfWork.Submissions
                .Query()
                .Include(s => s.Assignment)
                .FirstOrDefaultAsync(s =>
                    s.Id == request.SubmissionId &&
                    !s.IsDeleted,
                    cancellationToken);

            if (submission is null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Submission not found.");
            }

            if (submission.StudentId != studentId)
            {
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "You are not authorized to modify this submission.");
            }

            var assignment = submission.Assignment;

            var isEnrolled = await unitOfWork.Enrollments
                .Query()
                .AnyAsync(e =>
                    e.CourseId == assignment.CourseId &&
                    e.StudentId == studentId &&
                    e.Status == EnrollmentStatus.Active,
                    cancellationToken);

            if (!isEnrolled)
            {
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "You are not enrolled in this course.");
            }

            if (!assignment.IsPublished)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Assignment is not published.");
            }

            if (assignment.IsClosed)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Assignment is closed.");
            }

            if (assignment.OpenAt > DateTime.Now)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Assignment is not open yet.");
            }

            if (submission.Status == SubmissionStatus.Graded)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "This submission has already been graded.");
            }

            if (DateTime.UtcNow > assignment.DueDate &&
                !assignment.AllowLateSubmission)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Submission deadline has passed.");
            }

            var uploadResult = await fileService.UploadFileAsync(request.File);

            if (!uploadResult.IsSuccess)
            {
                return Result<string>.Failure(
                    uploadResult.Status,
                    uploadResult.Error);
            }

            var oldFile = submission.FileUrl;

            try
            {
                submission.FileName = request.File.FileName;
                submission.FileUrl = uploadResult.Value;
                submission.ContentType = request.File.ContentType;
                submission.FileSize = request.File.Length;
                submission.Comment = request.Comment;
                submission.SubmittedAt = DateTime.Now;

                var isLate = DateTime.Now > assignment.DueDate;

                submission.IsLate = isLate;
                submission.Status = isLate
                    ? SubmissionStatus.Late
                    : SubmissionStatus.Submitted;

                submission.UpdatedAt = DateTime.Now;
                submission.UpdatedBy = currentUserService.UserName;

                await unitOfWork.SaveAsync();

                fileService.Remove(oldFile);

                return Result<string>.Success(
                    "Submission updated successfully.");
            }
            catch
            {
                fileService.Remove(uploadResult.Value!);

                throw;
            }
        }
    }
}
