using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Exams.Queriess.GetMyExamResult;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Queriess.GetMyExamResults
{
    public class GetMyExamResultsQueryHandler
        : IRequestHandler<GetMyExamResultsQuery, Result<List<MyExamResultsDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetMyExamResultsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<MyExamResultsDto>>> Handle(
            GetMyExamResultsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<List<MyExamResultsDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required");
            }

            var userId = currentUserService.UserId;

            var results = await unitOfWork.ExamAttempts.Query()
                .Where(x =>
                    x.StudentId == userId &&
                    x.Status == ExamAttemptStatus.Completed)
                .OrderByDescending(x => x.SubmittedAt)
                .Select(x => new MyExamResultsDto
                {
                    AttemptId = x.Id,

                    ExamId = x.ExamId,

                    ExamTitle = x.Exam.Title,

                    Score = x.Score,

                    Percentage = x.Percentage,

                    IsPassed = x.IsPassed,

                    AttemptNumber = x.AttemptNumber,

                    SubmittedAt = x.SubmittedAt ?? x.EndTime ?? x.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return Result<List<MyExamResultsDto>>
                .Success(results);
        }
    }
}
