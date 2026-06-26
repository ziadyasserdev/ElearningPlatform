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

namespace ElearningPlatform.Application.Features.Exams.Commands.FinishExam
{
    public class FinishExamCommandHandler
    : IRequestHandler<FinishExamCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public FinishExamCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            FinishExamCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required");
            }

            var userId = currentUserService.UserId;
            var now = DateTime.UtcNow;

            var attempt = await unitOfWork.ExamAttempts.Query()
                .Include(x => x.Exam)
                .Include(x => x.StudentAnswers)
                .FirstOrDefaultAsync(
                    x => x.Id == request.AttemptId &&
                         x.StudentId == userId,
                    cancellationToken);

            if (attempt is null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Attempt not found");
            }

            if (attempt.Status != ExamAttemptStatus.InProgress)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Exam already finished");
            }

            var totalScore = attempt.StudentAnswers.Sum(x => x.ScoreAwarded);

            attempt.Score = (int)totalScore;

            attempt.Percentage =
                attempt.Exam.TotalScore == 0
                ? 0
                : (decimal)totalScore / attempt.Exam.TotalScore * 100;

            attempt.IsPassed =
                totalScore >= attempt.Exam.PassingScore;

            attempt.Status = ExamAttemptStatus.Completed;

            attempt.SubmittedAt = now;
            attempt.EndTime = now;

            attempt.UpdatedAt = now;
            attempt.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<string>.Success(
                "Exam submitted successfully");
        }
    }

}
