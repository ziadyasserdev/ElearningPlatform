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

namespace ElearningPlatform.Application.Features.Videos.Commands.BulkRestoreVideos
{
    public class BulkRestoreVideosCommandHandler : IRequestHandler<BulkRestoreVideosCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public BulkRestoreVideosCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(BulkRestoreVideosCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            
            var videos = await unitOfWork.Videos.Query()
                .Where(x => request.VideoIds.Contains(x.Id) && x.IsDeleted)
                .Include(x => x.Lesson)
                    .ThenInclude(l => l.Section)
                        .ThenInclude(s => s.Course)
                .ToListAsync(cancellationToken);

            if (!videos.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "No deleted videos found");

    
            if (videos.Any(v => v.Lesson.Section.Course.Instructor.UserId != userId))
                return Result<string>.Failure(ResultStatus.Forbidden, "Not allowed");

            var now = DateTime.Now;
            var username = currentUserService.UserName;

          
            var groupedBySection = videos.GroupBy(v => v.Lesson.SectionId);

            foreach (var group in groupedBySection)
            {
                var sectionId = group.Key;

              
                var maxOrder = await unitOfWork.Videos.Query()
                    .Where(v => v.Lesson.SectionId == sectionId && !v.IsDeleted)
                    .MaxAsync(v => (int?)v.Order, cancellationToken) ?? 0;

                var orderedIds = group.Select(x => x.Id).ToList();

               
                foreach (var videoId in orderedIds)
                {
                    maxOrder++;

                    await unitOfWork.Videos.Query()
                        .Where(v => v.Id == videoId)
                        .ExecuteUpdateAsync(setters => setters
                            .SetProperty(v => v.IsDeleted, false)
                            .SetProperty(v => v.Order, maxOrder)
                            .SetProperty(v => v.UpdatedAt, now)
                            .SetProperty(v => v.UpdatedBy, username),
                        cancellationToken);
                }
            }

            return Result<string>.Success("Videos restored successfully");
        }
    }
}
