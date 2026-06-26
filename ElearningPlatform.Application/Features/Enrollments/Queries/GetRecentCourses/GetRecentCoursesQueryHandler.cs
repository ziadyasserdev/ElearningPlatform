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

namespace ElearningPlatform.Application.Features.Enrollments.Queries.GetRecentCourses
{
    public class GetRecentCoursesQueryHandler
      : IRequestHandler<GetRecentCoursesQuery, Result<List<RecentCourseDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetRecentCoursesQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<List<RecentCourseDto>>> Handle(
            GetRecentCoursesQuery request,
            CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var query = unitOfWork.VideoProgresses.Query()
                .Where(v => v.UserId == userId)
                .Include(v => v.Video)
                    .ThenInclude(v => v.Lesson)
                        .ThenInclude(l => l.Section)
                            .ThenInclude(s => s.Course);

            var latestPerCourse = await query
                .GroupBy(v => v.Video.Lesson.Section.CourseId)
                .Select(g => g.OrderByDescending(x => x.LastWatchedAt).First())
                .OrderByDescending(x => x.LastWatchedAt)
                .ToListAsync(cancellationToken);

            if (!latestPerCourse.Any())
            {
                return Result<List<RecentCourseDto>>.Failure(
                    ResultStatus.NotFound,
                    "No recent courses found");
            }

            var scheme = httpContextAccessor.HttpContext!.Request.Scheme;
            var host = httpContextAccessor.HttpContext!.Request.Host;

            var result = latestPerCourse.Select(v => new RecentCourseDto
            {
                CourseId = v.Video.Lesson.Section.CourseId,
                CourseTitle = v.Video.Lesson.Section.Course.Title,
                CourseThumbnail =
                    $"{scheme}://{host}/{v.Video.Lesson.Section.Course.ThumbnailUrl}",

                LastVideoId = v.VideoId,
                LastVideoTitle = v.Video.Title,

                ProgressPercentage = v.ProgressPercentage,
                LastWatchedAt = v.LastWatchedAt
            }).ToList();

            return Result<List<RecentCourseDto>>.Success(result);
        }
    }
}
