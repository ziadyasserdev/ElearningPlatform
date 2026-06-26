using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Videos.Dtos;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Queries.GetVideosByLessonId
{
    public class GetVideosByLessonIdQueryHandler : IRequestHandler<GetVideosByLessonIdQuery, Result<List<VideoListDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetVideosByLessonIdQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<List<VideoListDto>>> Handle(GetVideosByLessonIdQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            var isInstructor = currentUserService.IsInRole("Instructor");

        
            var lesson = await unitOfWork.Lessons.Query()
                .Include(l => l.Section)
                .ThenInclude(s => s.Course)
                .ThenInclude(x => x.Instructor)
                .FirstOrDefaultAsync(l =>
                    l.Id == request.LessonId && !l.IsDeleted,
                    cancellationToken);

            if (lesson is null)
                return Result<List<VideoListDto>>.Failure(ResultStatus.NotFound, "Lesson not found");

            var isOwner = lesson.Section.Course.Instructor.UserId == userId;

            var query = unitOfWork.Videos.Query()
                .Where(v => v.LessonId == request.LessonId && !v.IsDeleted);

         
            if (!isOwner)
            {
                if (!lesson.IsPublished)
                    return Result<List<VideoListDto>>.Failure(ResultStatus.Forbidden, "Lesson not accessible");

                query = query.Where(v => v.ProcessingStatus == VideoProcessingStatus.Ready);
            }

            var videos = await query
                .OrderBy(v => v.CreatedAt)
                .Select(v => new VideoListDto
                {
                    Id = v.Id,
                    Title = v.Title,
                    Duration = v.Duration,
                    ProcessingStatus = v.ProcessingStatus
                })
                .ToListAsync(cancellationToken);

            return Result<List<VideoListDto>>.Success(videos);
        }
    }
}
