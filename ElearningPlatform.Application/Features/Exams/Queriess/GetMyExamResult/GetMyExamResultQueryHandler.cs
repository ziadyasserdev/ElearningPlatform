using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Exams.Dtos;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Queriess.GetMyExamResult
{
    public class GetExamResultQueryHandler
      : IRequestHandler<GetMyExamResultQuery, Result<MyExamResultDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetExamResultQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<MyExamResultDto>> Handle(
            GetMyExamResultQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<MyExamResultDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required");
            }

            var userId = currentUserService.UserId;

            var result = await unitOfWork.ExamAttempts.Query()
                .Where(x =>
                    x.Id == request.AttemptId &&
                    x.StudentId == userId &&
                    x.Status == ExamAttemptStatus.Completed)
                .Select(x => new MyExamResultDto
                {
                    AttemptId = x.Id,

                    ExamTitle = x.Exam.Title,

                    Score = x.Score,

                    Percentage = x.Percentage,

                    IsPassed = x.IsPassed,

                    TotalQuestions = x.Exam.Questions.Count,

                    CorrectAnswers = x.StudentAnswers.Count(a =>
                        a.ScoreAwarded > 0),

                    WrongAnswers = x.StudentAnswers.Count(a =>
                        a.ScoreAwarded <= 0),

                    StartTime = x.StartTime,

                    EndTime = x.EndTime,

                    Duration = x.EndTime.HasValue
                        ? x.EndTime.Value - x.StartTime
                        : TimeSpan.Zero
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (result is null)
            {
                return Result<MyExamResultDto>.Failure(
                    ResultStatus.NotFound,
                    "Result not found");
            }

            return Result<MyExamResultDto>.Success(result);
        }
    }
}
