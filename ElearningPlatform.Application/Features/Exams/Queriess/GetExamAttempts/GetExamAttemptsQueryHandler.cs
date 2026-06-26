using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Exams.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Queriess.GetExamAttempts
{
    public class GetExamAttemptsQueryHandler
         : IRequestHandler<GetExamAttemptsQuery, Result<List<ExamAttemptDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetExamAttemptsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<ExamAttemptDto>>> Handle(
            GetExamAttemptsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var attempts = await unitOfWork.ExamAttempts.Query()
                .Where(x =>
                    x.ExamId == request.Id &&
                    x.Exam.Course.Instructor.UserId == userId)
                .Select(x => new ExamAttemptDto
                {
                    AttemptId = x.Id,
                    StudentName = x.Student.FullName,
                    StudentEmail = x.Student.Email!,
                    Score = x.Score,
                    Status = x.Status.ToString(),
                    StartTime = x.StartTime,
                    EndTime = x.EndTime
                })
                .ToListAsync(cancellationToken);

            if (attempts == null || attempts.Count == 0)
                return Result<List<ExamAttemptDto>>.Failure(
                    ResultStatus.NotFound,
                    "No attempts found");

            return Result<List<ExamAttemptDto>>.Success(attempts);
        }
    }
}
