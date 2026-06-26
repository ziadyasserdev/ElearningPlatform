using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Enums;
using ElearningPlatform.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.UpdateVideoProgress
{
    public class UpdateVideoProgressCommandHandler
       : IRequestHandler<UpdateVideoProgressCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public UpdateVideoProgressCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            UpdateVideoProgressCommand request,
            CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            var now = DateTime.UtcNow;

            var video = await unitOfWork.Videos.Query()
                .Include(v => v.Lesson)
                .ThenInclude(l => l.Section)
                .FirstOrDefaultAsync(
                    v => v.Id == request.VideoId && !v.IsDeleted,
                    cancellationToken);

            if (video is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Video not found");

            var courseId = video.Lesson.Section.CourseId;

            var enrollment = await unitOfWork.Enrollments.Query()
                .FirstOrDefaultAsync(
                    e => e.CourseId == courseId &&
                         e.StudentId == userId,
                    cancellationToken);

            if (enrollment is null)
                return Result<string>.Failure(ResultStatus.Forbidden, "You are not enrolled in this course");

         
            var progress = await unitOfWork.VideoProgresses.Query()
                .FirstOrDefaultAsync(
                    x => x.VideoId == request.VideoId &&
                         x.UserId == userId,
                    cancellationToken);

            if (progress is null)
            {
                progress = new VideoProgress
                {
                    UserId = userId,
                    VideoId = request.VideoId,
                    CreatedAt = now,
                    VideoDuration = video.Duration
                };

                await unitOfWork.VideoProgresses.AddAsync(progress);
            }

            if (request.CurrentTimeInSeconds > progress.LastWatchedSecond)
            {
                progress.LastWatchedSecond = request.CurrentTimeInSeconds;
                progress.LastWatchedAt = now;
            }

            progress.WatchedSeconds =
                Math.Max(progress.WatchedSeconds, request.CurrentTimeInSeconds);

            progress.VideoDuration = video.Duration;

            if (progress.VideoDuration > 0)
            {
                var percent =
                    (double)progress.LastWatchedSecond /
                    progress.VideoDuration * 100;

                progress.ProgressPercentage = Math.Min(percent, 100);
            }

            progress.IsCompleted = progress.ProgressPercentage >= 90;
            progress.UpdatedAt = now;

           

            var courseVideoIds = await unitOfWork.Videos.Query()
                .Where(v => v.Lesson.Section.CourseId == courseId && !v.IsDeleted)
                .Select(v => v.Id)
                .ToListAsync(cancellationToken);

            var allProgress = await unitOfWork.VideoProgresses.Query()
                .Where(v =>
                    v.UserId == userId &&
                    courseVideoIds.Contains(v.VideoId))
                .ToListAsync(cancellationToken);

            var totalVideos = courseVideoIds.Count;

            var completedVideos = allProgress
                .Count(x => x.ProgressPercentage >= 90);

            var completionRate =
                totalVideos == 0
                    ? 0
                    : (double)completedVideos / totalVideos * 100;

            enrollment.ProgressPercentage = (decimal)completionRate;

            if (completionRate >= 100 &&
                enrollment.Status != EnrollmentStatus.Completed)
            {
                enrollment.Status = EnrollmentStatus.Completed;
                enrollment.CompletedAt = now;
            }

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Progress updated successfully");
        }
    }
}
