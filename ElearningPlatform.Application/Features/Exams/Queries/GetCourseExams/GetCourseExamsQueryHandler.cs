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

namespace ElearningPlatform.Application.Features.Exams.Queries.GetCourseExams
{
    public class GetCourseExamsQueryHandler
      : IRequestHandler<GetCourseExamsQuery, Result<List<ExamDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetCourseExamsQueryHandler(
            IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<ExamDto>>> Handle(
            GetCourseExamsQuery request,
            CancellationToken cancellationToken)
        {
            

            if(!currentUserService.IsAuthenticated)
                return Result<List<ExamDto>>.Failure(ResultStatus.Unauthorized, "Authentication required");
            var userId = currentUserService.UserId;

            var courseExists = await unitOfWork.Courses.Query()
                .AnyAsync(x =>
                    x.Id == request.CourseId &&
                    x.Instructor.UserId == userId &&
                    !x.IsDeleted,
                    cancellationToken);

            if (!courseExists)
            {
                return Result<List<ExamDto>>.Failure(
                    ResultStatus.NotFound,
                    "Course not found");
            }

            var exams = await unitOfWork.Exams.Query()
                .Where(x =>
                    x.CourseId == request.CourseId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ExamDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    DurationMinutes = x.DurationMinutes,
                    TotalScore = x.TotalScore,
                    PassingScore = x.PassingScore,
                    IsActive = x.IsActive,
                    QuestionsCount = x.Questions.Count
                })
                .ToListAsync(cancellationToken);

            return Result<List<ExamDto>>.Success(exams);
        }
    }
}
