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

namespace ElearningPlatform.Application.Features.Exams.Queriess.GetActiveAttempt
{

    public class GetActiveAttemptQueryHandler
        : IRequestHandler<GetActiveAttemptQuery, Result<ActiveAttemptDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetActiveAttemptQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<ActiveAttemptDto>> Handle(
            GetActiveAttemptQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<ActiveAttemptDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required");
            }

            var userId = currentUserService.UserId;

            var attempt = await unitOfWork.ExamAttempts.Query()
                .Where(x =>
                    x.ExamId == request.ExamId &&
                    x.StudentId == userId &&
                    x.Status == ExamAttemptStatus.InProgress)
                .Select(x => new ActiveAttemptDto
                {
                    AttemptId = x.Id,
                    ExamId = x.ExamId,

                    StartTime = x.StartTime,

                    DurationMinutes = x.Exam.DurationMinutes,

                    EndsAt = x.StartTime
                        .AddMinutes(x.Exam.DurationMinutes),

                    RemainingSeconds =
                        (int)Math.Max(
                            0,
                            (x.StartTime
                                .AddMinutes(x.Exam.DurationMinutes)
                                - DateTime.UtcNow)
                            .TotalSeconds),

                    AnsweredQuestionsCount =
                        x.StudentAnswers.Count,

                    TotalQuestionsCount =
                        x.Exam.Questions.Count
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (attempt is null)
            {
                return Result<ActiveAttemptDto>.Failure(
                    ResultStatus.NotFound,
                    "No active attempt found");
            }

            return Result<ActiveAttemptDto>.Success(attempt);
        }
    }
}
