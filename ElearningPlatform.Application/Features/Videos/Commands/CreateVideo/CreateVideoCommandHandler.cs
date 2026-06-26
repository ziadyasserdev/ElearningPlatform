using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Contracts.Services;
using ElearningPlatform.Application.Features.Videos.Dtos;
using ElearningPlatform.Domain.Enums;
using ElearningPlatform.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.CreateVideo
{
    public class CreateVideoCommandHandler : IRequestHandler<CreateVideoCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediaService mediaService;
        private readonly ICurrentUserService currentUserService;
        private readonly IVideoProcessingService videoProcessingService;

        public CreateVideoCommandHandler(IUnitOfWork unitOfWork
            , IMediaService mediaService
            ,ICurrentUserService currentUserService
            ,IVideoProcessingService videoProcessingService)
        {
            this.unitOfWork = unitOfWork;
            this.mediaService = mediaService;
            this.currentUserService = currentUserService;
            this.videoProcessingService = videoProcessingService;
        }
       
        public async Task<Result<int>> Handle(CreateVideoCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var lesson = await unitOfWork.Lessons.Query()
                .Include(l => l.Section)
                .ThenInclude(s => s.Course)
                .ThenInclude(c => c.Instructor)
                .FirstOrDefaultAsync(x =>
                    x.Id == request.LessonId &&
                    !x.IsDeleted,
                    cancellationToken);

            if (lesson is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Lesson not found");

          
            if (lesson.Section.Course.Instructor.UserId != userId)
                return Result<int>.Failure(ResultStatus.Unauthorized, "Not allowed");

          
            var uploadResult = await mediaService.UploadVideoAsync(request.File);

            if (!uploadResult.IsSuccess)
                return Result<int>.Failure(uploadResult.Status, uploadResult.Message);
            var maxOrder = await unitOfWork.Videos.Query()
                .Where(v => v.LessonId == request.LessonId && !v.IsDeleted)
                .MaxAsync(v => (int?)v.Order) ?? 0;
            var video = new Video
            {
                Title = request.Title,
                FileUrl = uploadResult.Value!.Url,
                FileSize = request.File.Length,
                Format = Path.GetExtension(request.File.FileName),

                ProcessingStatus = VideoProcessingStatus.Pending,

                UploadedBy = currentUserService.UserId!,
                LessonId = request.LessonId,

                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUserService.UserName,

                Order = maxOrder + 1,

                Duration = request.Duration
            };

            await unitOfWork.Videos.AddAsync(video);

            lesson.Duration = (lesson.Duration ?? 0) + video.Duration;

            lesson.UpdatedAt = DateTime.UtcNow;
            lesson.UpdatedBy = currentUserService.UserName;

            var course = lesson.Section.Course;

            course.TotalDurationInMinutes += video.Duration;

            course.UpdatedAt = DateTime.UtcNow;
            course.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            _ = Task.Run(() => videoProcessingService.ProcessVideoAsync(video.Id));

            return Result<int>.Success(video.Id);
            //var video = new Video
            //{
            //    Title = request.Title,
            //    FileUrl = uploadResult.Value!.Url,
            //    FileSize = request.File.Length,
            //    Format = Path.GetExtension(request.File.FileName),

            //    ProcessingStatus = VideoProcessingStatus.Pending,

            //    UploadedBy = currentUserService.UserId!,
            //    LessonId = request.LessonId,

            //    CreatedAt = DateTime.Now,
            //    CreatedBy = currentUserService.UserName,
            //    Order = maxOrder + 1,
            //    //Duration = 0
            //    Duration = request.Duration 
            //};

            //await unitOfWork.Videos.AddAsync(video);
            //lesson.Duration = lesson.Videos.Where(v => !v.IsDeleted).Sum(v => v.Duration) + video.Duration;
            //await unitOfWork.SaveAsync();
            //_ = Task.Run(() => videoProcessingService.ProcessVideoAsync(video.Id));
            //return Result<int>.Success(video.Id);

        }
    }
}
