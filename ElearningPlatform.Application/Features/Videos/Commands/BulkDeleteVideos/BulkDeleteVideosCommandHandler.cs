using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.BulkDeleteVideos
{
    public class BulkDeleteVideosCommandHandler : IRequestHandler<BulkDeleteVideosCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public BulkDeleteVideosCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(BulkDeleteVideosCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            var now = DateTime.UtcNow;
            var username = currentUserService.UserName;

          
            var videos = await unitOfWork.Videos.Query()
                .Where(x => request.VideoIds.Contains(x.Id) && !x.IsDeleted)
                .Include(x => x.Lesson)
                    .ThenInclude(l => l.Section)
                        .ThenInclude(s => s.Course)
                .ToListAsync(cancellationToken);

            if (!videos.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "No videos found");

          
            if (videos.Any(v => v.Lesson.Section.Course.Instructor.UserId != userId))
                return Result<string>.Failure(ResultStatus.Forbidden, "Not allowed");

        
            var videosWithProgress = await unitOfWork.Videos.Query()
                .Where(x => request.VideoIds.Contains(x.Id))
                .Where(x => x.VideoProgresses.Any())
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);

         
            var validIds = videos
                .Where(x => !videosWithProgress.Contains(x.Id))
                .Select(x => x.Id)
                .ToList();

            if (!validIds.Any())
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "All selected videos have progress and cannot be deleted");

           
            var updatedCount = await unitOfWork.Videos.Query()
                .Where(v => validIds.Contains(v.Id))
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(v => v.IsDeleted, true)
                    .SetProperty(v => v.UpdatedAt, now)
                    .SetProperty(v => v.UpdatedBy, username),
                cancellationToken);




         
            return Result<string>.Success($"{updatedCount} videos deleted successfully");
        }
    }
}
