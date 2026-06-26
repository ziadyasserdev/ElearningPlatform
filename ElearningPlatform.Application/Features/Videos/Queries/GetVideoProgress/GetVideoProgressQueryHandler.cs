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

namespace ElearningPlatform.Application.Features.Videos.Queries.GetVideoProgress
{
    public class GetVideoProgressQueryHandler
    : IRequestHandler<GetVideoProgressQuery, Result<VideoProgressDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetVideoProgressQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<VideoProgressDto>> Handle(
            GetVideoProgressQuery request,
            CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var progress = await unitOfWork.VideoProgresses.Query()
                .FirstOrDefaultAsync(p =>
                    p.VideoId == request.VideoId &&
                    p.UserId == userId,
                    cancellationToken);

            if (progress is null)
                return Result<VideoProgressDto>.Failure(ResultStatus.NotFound, "No progress found");

            return Result<VideoProgressDto>.Success(new VideoProgressDto
            {
                WatchedSeconds = progress.WatchedSeconds,
                ProgressPercentage = progress.ProgressPercentage,
                IsCompleted = progress.IsCompleted,
                LastWatchedSecond = progress.LastWatchedSecond
            });
        }
    }
}
