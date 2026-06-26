using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Contracts.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.UpdateThumbnail
{
    public class UpdateThumbnailCommandHandler : IRequestHandler<UpdateThumbnailCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IFileService fileService;

        public UpdateThumbnailCommandHandler(IUnitOfWork unitOfWork
            , ICurrentUserService currentUserService
            , IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.fileService = fileService;
        }
        public async Task<Result<int>> Handle(UpdateThumbnailCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var videoData = await unitOfWork.Videos.Query()
                .Where(x => x.Id == request.VideoId && !x.IsDeleted)
                .Select(x => new
                {
                    Video = x,
                    InstructorId = x.Lesson.Section.Course.Instructor.UserId
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (videoData == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Video not found");

            if (videoData.InstructorId != userId)
                return Result<int>.Failure(ResultStatus.Forbidden, "Not allowed");
            var video = videoData.Video;


            if (!string.IsNullOrEmpty(video.ThumbnailUrl))
            {
                fileService.Remove(video.ThumbnailUrl);
            }


            var uploadResult = await fileService.UploadImageAsync(request.Thumbnail);

            if (!uploadResult.IsSuccess || uploadResult.Value == null)
                return Result<int>.Failure(uploadResult.Status, uploadResult.Message ?? "Thumbnail upload failed");

            video.ThumbnailUrl = uploadResult.Value;
            video.UpdatedAt = DateTime.UtcNow;
            video.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<int>.Success(video.Id, "Thumbnail updated successfully");
        }
    }
}
