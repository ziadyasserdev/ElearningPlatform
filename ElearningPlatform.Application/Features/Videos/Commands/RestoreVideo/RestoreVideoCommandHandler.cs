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

namespace ElearningPlatform.Application.Features.Videos.Commands.RestoreVideo
{
    public class RestoreVideoCommandHandler : IRequestHandler<RestoreVideoCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public RestoreVideoCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(RestoreVideoCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var video = await unitOfWork.Videos.Query()
                .Where(x => x.Id == request.VideoId && x.IsDeleted)
                .Select(x => new
                {
                    Video = x,
                    SectionId = x.Lesson.SectionId,
                    InstructorId = x.Lesson.Section.Course.Instructor.UserId,
                    Title = x.Title
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (video == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Video not found or already active");

            if (video.InstructorId != userId)
                return Result<int>.Failure(ResultStatus.Forbidden, "Not allowed");

          
            var checkTitle = await unitOfWork.Videos.Query()
                .AnyAsync(x =>
                    x.Id != request.VideoId &&
                    !x.IsDeleted &&
                    x.Lesson.SectionId == video.SectionId &&
                    x.Title == video.Title,
                    cancellationToken);

            if (checkTitle)
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "A video with the same title already exists in this section. Please rename the video before restoring.");

           
            var maxOrder = await unitOfWork.Videos.Query()
                .Where(v => v.Lesson.SectionId == video.SectionId && !v.IsDeleted)
                .MaxAsync(v => (int?)v.Order, cancellationToken) ?? 0;

            var entity = video.Video;

            entity.IsDeleted = false;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = currentUserService.UserName;
            entity.Order = maxOrder + 1;

            await unitOfWork.SaveAsync();

            return Result<int>.Success(entity.Id, "Video restored successfully");
        }
    }
}
