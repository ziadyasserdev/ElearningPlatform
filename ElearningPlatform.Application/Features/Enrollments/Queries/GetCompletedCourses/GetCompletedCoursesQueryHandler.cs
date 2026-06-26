using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Enrollments.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Queries.GetCompletedCourses
{
    public class GetCompletedCoursesQueryHandler
          : IRequestHandler<GetCompletedCoursesQuery, Result<List<CompletedCourseDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetCompletedCoursesQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<List<CompletedCourseDto>>> Handle(
            GetCompletedCoursesQuery request,
            CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<List<CompletedCourseDto>>.Failure(ResultStatus.Unauthorized, "Authentication required");



            var userId = currentUserService.UserId;

            var enrollments = await unitOfWork.Enrollments.Query()
                .Where(e =>
                    e.StudentId == userId &&
                    e.CompletedAt != null)
                .Include(e => e.Course)
                .ToListAsync(cancellationToken);

            if (!enrollments.Any())
                return Result<List<CompletedCourseDto>>.Failure(
                    ResultStatus.NotFound,
                    "No completed courses found");

            var scheme = httpContextAccessor.HttpContext!.Request.Scheme;
            var host = httpContextAccessor.HttpContext!.Request.Host;

            var result = enrollments.Select(e => new CompletedCourseDto
            {
                CourseId = e.CourseId,
                CourseTitle = e.Course.Title,
                CourseThumbnail = $"{scheme}://{host}/{e.Course.ThumbnailUrl}",
                ProgressPercentage = e.ProgressPercentage,
                CompletedAt = e.CompletedAt ?? DateTime.UtcNow
            }).ToList();

            return Result<List<CompletedCourseDto>>.Success(result);
        }
    }
}
