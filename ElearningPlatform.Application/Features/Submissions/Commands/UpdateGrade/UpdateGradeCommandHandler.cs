using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Commands.UpdateGrade
{
    public class UpdateGradeCommandHandler
       : IRequestHandler<UpdateGradeCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public UpdateGradeCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            UpdateGradeCommand request,
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
                        .ThenInclude(c => c.Instructor)
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

            if (submission.Assignment.Course.Instructor.UserId != userId)
            {
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "You are not authorized to update this grade.");
            }

            if (submission.Status != SubmissionStatus.Graded)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Submission has not been graded yet.");
            }

            if (!submission.Assignment.IsPublished)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Assignment is not published.");
            }

            if (request.Score > submission.Assignment.MaxScore)
            {
                return Result<string>.Failure(
                    ResultStatus.ValidationError,
                    $"Score cannot exceed {submission.Assignment.MaxScore}.");
            }

            submission.Score = request.Score;
            submission.Feedback = request.Feedback;

         
            submission.GradedAt = DateTime.Now;

            submission.UpdatedAt = DateTime.Now;
            submission.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<string>.Success(
                "Grade updated successfully.");
        }
    }
}
