using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Contracts.Services;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.UpdateVideo
{
    public class UpdateVideoCommandHandler
       : IRequestHandler<UpdateVideoCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IMediaService mediaService;

        public UpdateVideoCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IMediaService mediaService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.mediaService = mediaService;
        }

        public async Task<Result<int>> Handle(
            UpdateVideoCommand request,
            CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var videoInfo = await unitOfWork.Videos.Query()
                .Where(x => x.Id == request.Id && !x.IsDeleted)
                .Select(x => new
                {
                    Video = x,
                    LessonId = x.LessonId,
                    InstructorId = x.Lesson.Section.Course.Instructor.UserId,
                    CourseId = x.Lesson.Section.CourseId
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (videoInfo is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Video not found");

            if (videoInfo.InstructorId != userId)
                return Result<int>.Failure(ResultStatus.Forbidden, "Not allowed");

            var video = await unitOfWork.Videos.Query()
                .FirstAsync(x => x.Id == request.Id, cancellationToken);

            // -------------------
            // update file if exists
            // -------------------
            if (request.File != null)
            {
                if (video.ProcessingStatus == VideoProcessingStatus.Processing)
                    return Result<int>.Failure(ResultStatus.Forbidden, "Cannot update video while processing");

                if (video.FileUrl == null)
                    return Result<int>.Failure(ResultStatus.Failure, "Existing video file is missing");

                var removeResult = await mediaService.DeleteVideoAsync(video.FileUrl);
                if (!removeResult.IsSuccess)
                    return Result<int>.Failure(removeResult.Status, removeResult.Message ?? "Delete failed");

                var uploadResult = await mediaService.UploadVideoAsync(request.File);
                if (!uploadResult.IsSuccess)
                    return Result<int>.Failure(uploadResult.Status, uploadResult.Message ?? "Upload failed");

                video.FileUrl = uploadResult.Value!.Url;
                video.Format = uploadResult.Value.ContentType;
                video.FileSize = uploadResult.Value.Size;
                video.ProcessingStatus = VideoProcessingStatus.Pending;
            }

            // -------------------
            // duration update
            // -------------------
            var oldDuration = video.Duration;
            video.Duration = request.Duration;

            // -------------------
            // basic fields
            // -------------------
            video.Title = request.Title;
            video.UpdatedAt = DateTime.UtcNow;
            video.UpdatedBy = currentUserService.UserName;

            // -------------------
            // recalc lesson + course
            // -------------------
            var lesson = await unitOfWork.Lessons.Query()
                .Include(l => l.Section)
                .ThenInclude(s => s.Course)
                .FirstOrDefaultAsync(l => l.Id == video.LessonId, cancellationToken);

            if (lesson != null)
            {
                lesson.Duration = await unitOfWork.Videos.Query()
                    .Where(v => !v.IsDeleted && v.LessonId == lesson.Id)
                    .SumAsync(v => (int?)v.Duration, cancellationToken) ?? 0;

                lesson.UpdatedAt = DateTime.UtcNow;
                lesson.UpdatedBy = currentUserService.UserName;

                var course = lesson.Section.Course;

                course.TotalDurationInMinutes = await unitOfWork.Videos.Query()
                    .Where(v =>
                        !v.IsDeleted &&
                        !v.Lesson.IsDeleted &&
                        v.Lesson.Section.CourseId == course.Id)
                    .SumAsync(v => (int?)v.Duration, cancellationToken) ?? 0;

                course.UpdatedAt = DateTime.UtcNow;
                course.UpdatedBy = currentUserService.UserName;
            }

            await unitOfWork.SaveAsync();

            return Result<int>.Success(video.Id, "Video updated successfully");
        }
    }
    //public class UpdateVideoCommandHandler : IRequestHandler<UpdateVideoCommand, Result<int>>
    //{
    //    private readonly IUnitOfWork unitOfWork;
    //    private readonly ICurrentUserService currentUserService;
    //    private readonly IMediaService mediaService;

    //    public UpdateVideoCommandHandler(IUnitOfWork unitOfWork
    //        ,ICurrentUserService currentUserService
    //        ,IMediaService mediaService)
    //    {
    //        this.unitOfWork = unitOfWork;
    //        this.currentUserService = currentUserService;
    //        this.mediaService = mediaService;
    //    }
    //    public async Task<Result<int>> Handle(UpdateVideoCommand request, CancellationToken cancellationToken)
    //    {
    //        var userId = currentUserService.UserId;
    //        var VideoInfo = await unitOfWork.Videos.Query()
    //            .Where(x => x.Id == request.Id
    //             && !x.IsDeleted)
    //            .Select(x => new
    //            {
    //                Video = x,
    //                InstructorId = x.Lesson.Section.Course.Instructor.UserId

    //            }).FirstOrDefaultAsync(cancellationToken);
    //        if(VideoInfo is null)
    //            return Result<int>.Failure(ResultStatus.NotFound, "Video not found");

    //        if(VideoInfo.InstructorId != userId)
    //            return Result<int>.Failure(ResultStatus.Forbidden, "Not allowed");
    //        if(request.File != null)
    //        {
    //             if(VideoInfo.Video.ProcessingStatus == VideoProcessingStatus.Processing)
    //                return Result<int>.Failure(ResultStatus.Forbidden, "Cannot update video while processing");
    //             if(VideoInfo.Video.FileUrl == null)
    //                return Result<int>.Failure(ResultStatus.Failure, "Existing video file is missing");

    //            var removeResult = await mediaService.DeleteVideoAsync(VideoInfo.Video.FileUrl);
    //            if(!removeResult.IsSuccess)
    //                return Result<int>.Failure(removeResult.Status, removeResult.Message ?? "Existing video deletion failed");
    //            var uploadResult = await mediaService.UploadVideoAsync(request.File);
    //            if(!uploadResult.IsSuccess)
    //                return Result<int>.Failure(uploadResult.Status, uploadResult.Message ?? "Video upload failed");
    //            VideoInfo.Video.FileUrl = uploadResult.Value!.Url;
    //                VideoInfo.Video.Format = uploadResult.Value.ContentType;
    //            VideoInfo.Video.FileSize = uploadResult.Value.Size;
    //            VideoInfo.Video.ProcessingStatus = VideoProcessingStatus.Pending;

    //        }

    //        VideoInfo.Video.Title = request.Title ?? VideoInfo.Video.Title;
    //        VideoInfo.Video.UpdatedAt = DateTime.UtcNow;
    //        VideoInfo.Video.UpdatedBy = currentUserService.UserName;
    //        await unitOfWork.SaveAsync();
    //        return Result<int>.Success(VideoInfo.Video.Id, "Video updated successfully");
    //    }
    //}
}
