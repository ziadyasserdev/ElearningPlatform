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

namespace ElearningPlatform.Application.Features.Enrollments.Commands.MarkCourseAsCompleted
{
    public class MarkCourseAsCompletedCommandHandler
         : IRequestHandler<MarkCourseAsCompletedCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public MarkCourseAsCompletedCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            MarkCourseAsCompletedCommand request,
            CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var enrollment = await unitOfWork.Enrollments.Query()
                .FirstOrDefaultAsync(e =>
                    e.CourseId == request.CourseId &&
                    e.StudentId == userId,
                    cancellationToken);

            if (enrollment is null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Enrollment not found");
            }

            if (enrollment.Status == EnrollmentStatus.Completed)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Course already completed");
            }

            var totalVideos = await unitOfWork.Videos.Query()
                .CountAsync(v =>
                    !v.IsDeleted &&
                    v.Lesson.Section.CourseId == request.CourseId,
                    cancellationToken);

            if (totalVideos == 0)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Course has no videos");
            }

            var completedVideos = await unitOfWork.VideoProgresses.Query()
                .CountAsync(v =>
                    v.UserId == userId &&
                    v.IsCompleted &&
                    v.Video.Lesson.Section.CourseId == request.CourseId,
                    cancellationToken);

            if (completedVideos < totalVideos)
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    $"Course is not completed yet. ({completedVideos}/{totalVideos}) videos completed");
            }

            enrollment.ProgressPercentage = 100;
            enrollment.Status = EnrollmentStatus.Completed;
            enrollment.CompletedAt = DateTime.UtcNow;

            enrollment.UpdatedAt = DateTime.UtcNow;
            enrollment.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<string>.Success(
                "Course marked as completed successfully");
        }
    }
}
