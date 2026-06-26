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

namespace ElearningPlatform.Application.Features.Enrollments.Queries.GetContinueWatching
{

    public class GetContinueWatchingQueryHandler
           : IRequestHandler<GetContinueWatchingQuery, Result<List<ContinueWatchingDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GetContinueWatchingQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<List<ContinueWatchingDto>>> Handle(
            GetContinueWatchingQuery request,
            CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var baseQuery = unitOfWork.VideoProgresses.Query()
                .Where(v =>
                    v.UserId == userId &&
                    !v.IsCompleted)
                .Include(v => v.Video)
                    .ThenInclude(v => v.Lesson)
                        .ThenInclude(l => l.Section)
                            .ThenInclude(s => s.Course);

            var grouped = await baseQuery
                .GroupBy(v => v.Video.Lesson.Section.CourseId)
                .Select(g => g
                    .OrderByDescending(x => x.LastWatchedAt)
                    .First())
                .ToListAsync(cancellationToken);

            if (!grouped.Any())
            {
                return Result<List<ContinueWatchingDto>>.Failure(
                    ResultStatus.NotFound,
                    "No continue watching items found");
            }

            var scheme = httpContextAccessor.HttpContext!.Request.Scheme;
            var host = httpContextAccessor.HttpContext!.Request.Host;

            var result = grouped.Select(v => new ContinueWatchingDto
            {
                CourseId = v.Video.Lesson.Section.CourseId,
                CourseTitle = v.Video.Lesson.Section.Course.Title,

                CourseThumbnail =
                    $"{scheme}://{host}/{v.Video.Lesson.Section.Course.ThumbnailUrl}",

                VideoId = v.VideoId,
                VideoTitle = v.Video.Title,

                LastWatchedSecond = v.LastWatchedSecond,
                ProgressPercentage = v.ProgressPercentage
            }).ToList();

            return Result<List<ContinueWatchingDto>>.Success(result);
        }
    }
}
