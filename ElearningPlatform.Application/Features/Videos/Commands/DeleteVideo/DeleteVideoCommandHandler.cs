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

namespace ElearningPlatform.Application.Features.Videos.Commands.DeleteVideo
{
    public class DeleteVideoCommandHandler
      : IRequestHandler<DeleteVideoCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public DeleteVideoCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            DeleteVideoCommand request,
            CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var videoInfo = await unitOfWork.Videos.Query()
                .Where(x => x.Id == request.Id && !x.IsDeleted)
                .Select(c => new
                {
                    Video = c,
                    LessonId = c.LessonId,
                    CourseId = c.Lesson.Section.CourseId,
                    InstructorId = c.Lesson.Section.Course.Instructor.UserId,
                    IsProcessing = c.ProcessingStatus == VideoProcessingStatus.Processing
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (videoInfo is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Video not found");

            if (videoInfo.InstructorId != userId)
                return Result<string>.Failure(ResultStatus.Forbidden, "Not allowed");

            var hasProgress = await unitOfWork.VideoProgresses.Query()
                .AnyAsync(vp => vp.VideoId == request.Id, cancellationToken);

            if (hasProgress)
                return Result<string>.Failure(ResultStatus.Forbidden, "Cannot delete video with user progress");

            if (videoInfo.IsProcessing)
                return Result<string>.Failure(ResultStatus.Forbidden, "Cannot delete video while processing");

            var now = DateTime.UtcNow;
            var userName = currentUserService.UserName;

           
            videoInfo.Video.IsDeleted = true;
            videoInfo.Video.DeletedAt = now;
            videoInfo.Video.DeletedBy = userName;

            videoInfo.Video.UpdatedAt = now;
            videoInfo.Video.UpdatedBy = userName;

          
            var lesson = await unitOfWork.Lessons.Query()
                .Include(l => l.Section)
                .ThenInclude(s => s.Course)
                .FirstOrDefaultAsync(l => l.Id == videoInfo.LessonId, cancellationToken);

            if (lesson != null)
            {
                lesson.Duration = await unitOfWork.Videos.Query()
                    .Where(v => !v.IsDeleted && v.LessonId == lesson.Id)
                    .SumAsync(v => (int?)v.Duration, cancellationToken) ?? 0;

                lesson.UpdatedAt = now;
                lesson.UpdatedBy = userName;

                var course = lesson.Section.Course;

                course.TotalDurationInMinutes = await unitOfWork.Videos.Query()
                    .Where(v =>
                        !v.IsDeleted &&
                        !v.Lesson.IsDeleted &&
                        v.Lesson.Section.CourseId == course.Id)
                    .SumAsync(v => (int?)v.Duration, cancellationToken) ?? 0;

                course.UpdatedAt = now;
                course.UpdatedBy = userName;
            }

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Video deleted successfully");
        }
    }
    //public class DeleteVideoCommandHandler : IRequestHandler<DeleteVideoCommand, Result<string>>
    //{
    //    private readonly IUnitOfWork unitOfWork;
    //    private readonly ICurrentUserService currentUserService;

    //    public DeleteVideoCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
    //    {
    //        this.unitOfWork = unitOfWork;
    //        this.currentUserService = currentUserService;
    //    }
    //    public async Task<Result<string>> Handle(DeleteVideoCommand request, CancellationToken cancellationToken)
    //    {
    //        var userId = currentUserService.UserId;

    //        var videoInfo = await unitOfWork.Videos.Query()
    //            .Where(x => x.Id == request.Id && !x.IsDeleted)
    //            .Select(c => new
    //            {
    //                Video = c,
    //                InstructorId = c.Lesson.Section.Course.Instructor.UserId,

    //                IsProcessing = c.ProcessingStatus == VideoProcessingStatus.Processing
    //            })
    //            .FirstOrDefaultAsync(cancellationToken);

    //        if (videoInfo is null)
    //            return Result<string>.Failure(ResultStatus.NotFound, "Video not found");

    //        if (videoInfo.InstructorId != userId)
    //            return Result<string>.Failure(ResultStatus.Forbidden, "Not allowed");
    //        var HasProgress = await unitOfWork.VideoProgresses.Query()
    //            .Where(vp => vp.VideoId == request.Id)
    //            .CountAsync(cancellationToken);
    //       if(HasProgress > 0)
    //            return Result<string>.Failure(ResultStatus.Forbidden, "Cannot delete video with user progress");



    //        if (videoInfo.IsProcessing)
    //            return Result<string>.Failure(ResultStatus.Forbidden, "Cannot delete video while processing");

    //        videoInfo.Video.IsDeleted = true;
    //        videoInfo.Video.DeletedAt = DateTime.Now;
    //        videoInfo.Video.DeletedBy = currentUserService.UserName;

    //        videoInfo.Video.UpdatedAt = DateTime.Now;
    //        videoInfo.Video.UpdatedBy = currentUserService.UserName;

    //        await unitOfWork.SaveAsync();

    //        return Result<string>.Success("Video deleted successfully");
    //    }
    //}
}
