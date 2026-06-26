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

namespace ElearningPlatform.Application.Features.Exams.Queriess.GetExamDetailsForInstructor
{
    public class GetExamDetailsForInstructorQueryHandler
         : IRequestHandler<GetExamDetailsForInstructorQuery, Result<ExamInstructorDetailsDto>>
    {
        private readonly ICurrentUserService currentUserService;
        private readonly IUnitOfWork unitOfWork;

        public GetExamDetailsForInstructorQueryHandler(
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            this.currentUserService = currentUserService;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<ExamInstructorDetailsDto>> Handle(
            GetExamDetailsForInstructorQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<ExamInstructorDetailsDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required");

            var userId = currentUserService.UserId;

            var exam = await unitOfWork.Exams.Query()
                .Where(x =>
                    x.Id == request.Id &&
                    x.Course.Instructor.UserId == userId)
                .Select(x => new ExamInstructorDetailsDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    StartAt = x.StartAt,
                    EndAt = x.EndAt,
                    DurationMinutes = x.DurationMinutes,
                    TotalScore = x.TotalScore,
                    PassingScore = x.PassingScore,
                 

                    QuestionsCount = x.Questions.Count,

                    TotalAttempts = x.ExamAttempts.Count,

                    CompletedAttempts = x.ExamAttempts.Count(
                        a => a.Status == ExamAttemptStatus.Completed),

                    PassedStudents = x.ExamAttempts.Count(
                        a => a.Score >= x.PassingScore),

                    FailedStudents = x.ExamAttempts.Count(
                        a => a.Score < x.PassingScore),

                    AverageScore = x.ExamAttempts.Any()
                        ? x.ExamAttempts.Average(a => a.Score)
                        : 0
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (exam == null)
            {
                return Result<ExamInstructorDetailsDto>.Failure(
                    ResultStatus.NotFound,
                    "Exam not found or not accessible");
            }

            return Result<ExamInstructorDetailsDto>.Success(exam);
        }
 
}
}
