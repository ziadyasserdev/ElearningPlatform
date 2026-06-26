using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.ReorderVideos
{
    public class ReorderVideosCommandHandler : IRequestHandler<ReorderVideosCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public ReorderVideosCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(ReorderVideosCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

           
            var section = await unitOfWork.Sections.Query()
                .Where(x => x.Id == request.SectionId && !x.IsDeleted && x.IsActive)
                .Select(x => new
                {
                    Section = x,
                    LessonIds = x.Lessons.Select(l => l.Id),
                    InstructorId = x.Course.Instructor.UserId
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (section == null)
                return Result<string>.Failure(ResultStatus.NotFound, "Section not found");

            if (section.InstructorId != userId)
                return Result<string>.Failure(ResultStatus.Forbidden, "Not allowed");

          
            var videos = await unitOfWork.Videos.Query()
                .Where(x => section.LessonIds.Contains(x.LessonId) && !x.IsDeleted)
                .ToListAsync(cancellationToken);

            if (!videos.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "No videos found");

           
            var requestedIds = request.Videos.Select(v => v.VideoId).ToList();

            if (videos.Count != requestedIds.Count)
                return Result<string>.Failure(ResultStatus.Failure, "Invalid video count");

          
            if (requestedIds.Distinct().Count() != requestedIds.Count)
                return Result<string>.Failure(ResultStatus.Failure, "Duplicate video IDs are not allowed");

          
            if (request.Videos.Select(v => v.NewOrder).Distinct().Count() != request.Videos.Count)
                return Result<string>.Failure(ResultStatus.Failure, "Duplicate order values are not allowed");

          
            var videoDict = videos.ToDictionary(v => v.Id);

          
            foreach (var item in request.Videos)
            {
                if (!videoDict.TryGetValue(item.VideoId, out var video))
                    return Result<string>.Failure(
                        ResultStatus.NotFound,
                        $"Video {item.VideoId} not found in this section");

                video.Order = item.NewOrder;
                video.UpdatedAt = DateTime.UtcNow;
                video.UpdatedBy = currentUserService.UserName;
            }

           
            await unitOfWork.SaveAsync();

            return Result<string>.Success("Videos reordered successfully");
        }
    }
}
