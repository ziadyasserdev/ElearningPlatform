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

namespace ElearningPlatform.Application.Features.Submissions.Commands.DeleteSubmission
{
    public class DeleteSubmissionCommandHandler
      : IRequestHandler<DeleteSubmissionCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IFileService fileService;

        public DeleteSubmissionCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.fileService = fileService;
        }

        public async Task<Result<string>> Handle(
            DeleteSubmissionCommand request,
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
                    "You are not authorized to delete this submission.");
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
            if (assignment.OpenAt > DateTime.Now)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Assignment is not open yet.");
            }

            if (assignment.IsClosed)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Assignment is closed.");
            }

            if (submission.Status == SubmissionStatus.Graded)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "This submission has already been graded.");
            }

            var removeResult = fileService.Remove(submission.FileUrl);

            if (!removeResult.IsSuccess)
            {
                return Result<string>.Failure(
                    removeResult.Status,
                    removeResult.Error);
            }

            submission.IsDeleted = true;
            submission.DeletedAt = DateTime.Now;
            submission.DeletedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<string>.Success(
                "Submission deleted successfully.");
        }
    }
}
