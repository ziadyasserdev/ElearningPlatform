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

namespace ElearningPlatform.Application.Features.Submissions.Commands.ResubmitSubmission
{
    public class ResubmitSubmissionCommandHandler
        : IRequestHandler<ResubmitSubmissionCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IFileService fileService;

        public ResubmitSubmissionCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.fileService = fileService;
        }

        public async Task<Result<string>> Handle(
            ResubmitSubmissionCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

            var userId = currentUserService.UserId;

            var submission = await unitOfWork.Submissions
                .Query()
                .Include(s => s.Assignment)
                .ThenInclude(a => a.Course)
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

            if (submission.StudentId != userId)
            {
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "You are not authorized to resubmit this assignment.");
            }

            var assignment = submission.Assignment;

            if (assignment.IsDeleted)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Assignment not found.");
            }

            if (DateTime.Now > assignment.DueDate)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "The submission deadline has passed.");
            }

            if (submission.Status != SubmissionStatus.Returned)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "This submission is not eligible for resubmission.");
            }

            var uploadResult = await fileService.UploadFileAsync(request.File);

            if (!uploadResult.IsSuccess)
            {
                return Result<string>.Failure(
                    uploadResult.Status,
                    uploadResult.Error);
            }

            var oldFileUrl = submission.FileUrl;

            try
            {
                submission.FileUrl = uploadResult.Value;
                submission.SubmittedAt = DateTime.Now;
                submission.AttemptNumber++;

                submission.Score = null;
                submission.Feedback = null;
                submission.GradedAt = null;

                submission.ReturnedAt = null;
                submission.ReturnReason = null;

                submission.Status = SubmissionStatus.Submitted;

                submission.UpdatedAt = DateTime.Now;
                submission.UpdatedBy = currentUserService.UserName;

                await unitOfWork.SaveAsync();

                if (!string.IsNullOrWhiteSpace(oldFileUrl))
                {
                    fileService.Remove(oldFileUrl);
                }

                return Result<string>.Success(
                    "Assignment resubmitted successfully.");
            }
            catch
            {
                fileService.Remove(uploadResult.Value);

                throw;
            }
        }
    }
}
