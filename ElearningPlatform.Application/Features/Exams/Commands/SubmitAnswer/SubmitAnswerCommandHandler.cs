using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Enums;
using ElearningPlatform.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Commands.SubmitAnswer
{
    public class SubmitAnswerCommandHandler
        : IRequestHandler<SubmitAnswerCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public SubmitAnswerCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            SubmitAnswerCommand request,
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
                    "Exam already finished");
            }

            var examExpired =
                attempt.StartTime.AddMinutes(attempt.Exam.DurationMinutes)
                < DateTime.UtcNow;

            if (examExpired)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Exam time expired");
            }

            var question = await unitOfWork.Questions.Query()
                .Include(x => x.Answers)
                .FirstOrDefaultAsync(x =>
                    x.Id == request.QuestionId &&
                    x.ExamId == attempt.ExamId,
                    cancellationToken);

            if (question is null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Question not found");
            }

            var studentAnswer = await unitOfWork.StudentAnswers.Query()
                .FirstOrDefaultAsync(x =>
                    x.ExamAttemptId == request.AttemptId &&
                    x.QuestionId == request.QuestionId,
                    cancellationToken);

            if (studentAnswer is null)
            {
                studentAnswer = new StudentAnswer
                {
                    ExamAttemptId = request.AttemptId,
                    QuestionId = request.QuestionId,
                    AnsweredAt = DateTime.UtcNow
                };

                await unitOfWork.StudentAnswers.AddAsync(studentAnswer);
            }

            studentAnswer.SelectedAnswerId = request.SelectedAnswerId;
            studentAnswer.TextAnswer = request.TextAnswer;
            studentAnswer.AnsweredAt = DateTime.UtcNow;

            if (request.SelectedAnswerId.HasValue)
            {
                var selectedAnswer = question.Answers
                    .FirstOrDefault(a =>
                        a.Id == request.SelectedAnswerId.Value);

                studentAnswer.ScoreAwarded =
                    selectedAnswer?.IsCorrect == true
                    ? question.Score
                    : 0;
            }

            await unitOfWork.SaveAsync();

            return Result<string>.Success(
                "Answer saved successfully");
        }
    }
}
