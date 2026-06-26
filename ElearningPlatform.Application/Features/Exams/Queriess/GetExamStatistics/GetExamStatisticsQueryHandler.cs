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

namespace ElearningPlatform.Application.Features.Exams.Queriess.GetExamStatistics
{

    public class GetExamStatisticsQueryHandler
        : IRequestHandler<GetExamStatisticsQuery, Result<ExamStatisticsDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetExamStatisticsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<ExamStatisticsDto>> Handle(
            GetExamStatisticsQuery request,
            CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<ExamStatisticsDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required");
            var userId = currentUserService.UserId;

            var exam = await unitOfWork.Exams.Query()
                .Where(x =>
                    x.Id == request.Id &&
                    x.Course.Instructor.UserId == userId)
                .Select(x => new ExamStatisticsDto
                {
                    ExamId = x.Id,

                    TotalAttempts = x.ExamAttempts.Count,

                    CompletedAttempts = x.ExamAttempts.Count(
                        a => a.Status == ExamAttemptStatus.Completed),

                    PassedStudents = x.ExamAttempts.Count(
                        a => a.Score >= x.PassingScore),

                    FailedStudents = x.ExamAttempts.Count(
                        a => a.Score < x.PassingScore),

                    AverageScore = x.ExamAttempts.Any()
                        ? x.ExamAttempts.Average(a => a.Score)
                        : 0,

                    PassRate = x.ExamAttempts.Any()
                        ? (double)x.ExamAttempts.Count(a => a.Score >= x.PassingScore)
                          / x.ExamAttempts.Count * 100
                        : 0
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (exam == null)
                return Result<ExamStatisticsDto>.Failure(
                    ResultStatus.NotFound,
                    "Exam not found or not accessible");

            return Result<ExamStatisticsDto>.Success(exam);
        }
    }
}
