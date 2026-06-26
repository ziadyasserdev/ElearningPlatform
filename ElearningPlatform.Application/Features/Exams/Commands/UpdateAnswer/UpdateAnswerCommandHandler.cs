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

namespace ElearningPlatform.Application.Features.Exams.Commands.UpdateAnswer
{
    public class UpdateAnswerCommandHandler
      : IRequestHandler<UpdateAnswerCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public UpdateAnswerCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            UpdateAnswerCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required");
            }

            var userId = currentUserService.UserId;

            var attempt = await unitOfWork.ExamAttempts.Query()
                .Include(x => x.Exam)
                .FirstOrDefaultAsync(x =>
                    x.Id == request.AttemptId &&
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
                    ResultStatus.Failure,
                    "Exam already submitted");
            }

            var answer = await unitOfWork.StudentAnswers.Query()
                .FirstOrDefaultAsync(x =>
                    x.ExamAttemptId == request.AttemptId &&
                    x.QuestionId == request.QuestionId,
                    cancellationToken);

            if (answer is null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Answer not found");
            }

            var question = await unitOfWork.Questions.Query()
                .Include(x => x.Answers)
                .FirstOrDefaultAsync(x =>
                    x.Id == request.QuestionId,
                    cancellationToken);

            if (question is null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Question not found");
            }

            answer.SelectedAnswerId = request.SelectedAnswerId;
            answer.TextAnswer = request.TextAnswer;
            answer.AnsweredAt = DateTime.UtcNow;

            if (request.SelectedAnswerId.HasValue)
            {
                var selectedAnswer = question.Answers
                    .FirstOrDefault(x =>
                        x.Id == request.SelectedAnswerId.Value);

                answer.ScoreAwarded =
                    selectedAnswer?.IsCorrect == true
                    ? question.Score
                    : 0;
            }
            else
            {
                answer.ScoreAwarded = 0;
            }

            await unitOfWork.SaveAsync();

            return Result<string>.Success(
                "Answer updated successfully");
        }
    }
}
