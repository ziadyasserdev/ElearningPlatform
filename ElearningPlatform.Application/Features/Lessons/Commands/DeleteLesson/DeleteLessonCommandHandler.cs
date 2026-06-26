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

namespace ElearningPlatform.Application.Features.Lessons.Commands.DeleteLesson
{




    public class DeleteLessonCommandHandler
      : IRequestHandler<DeleteLessonCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public DeleteLessonCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(
            DeleteLessonCommand request,
            CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var lesson = await unitOfWork.Lessons.Query()
                .Include(l => l.Section)
                .ThenInclude(s => s.Course)
                .ThenInclude(c => c.Instructor)
                .FirstOrDefaultAsync(
                    l => l.Id == request.Id &&
                         !l.IsDeleted,
                    cancellationToken);

            if (lesson is null)
            {
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    "Lesson not found");
            }

            if (lesson.Section.Course.Instructor.UserId != userId)
            {
                return Result<int>.Failure(
                    ResultStatus.Unauthorized,
                    "You are not allowed to delete this lesson");
            }

            var hasProgress = await unitOfWork.VideoProgresses.Query()
                .AnyAsync(
                    vp => vp.Video.LessonId == lesson.Id,
                    cancellationToken);

            if (hasProgress)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "Cannot delete lesson because students have progress on it");
            }

            var now = DateTime.UtcNow;
            var userName = currentUserService.UserName;

            await unitOfWork.Videos.Query()
                .Where(v =>
                    v.LessonId == lesson.Id &&
                    !v.IsDeleted)
                .ExecuteUpdateAsync(v => v
                    .SetProperty(x => x.IsDeleted, true)
                    .SetProperty(x => x.DeletedAt, now)
                    .SetProperty(x => x.DeletedBy, userName),
                    cancellationToken);

            lesson.IsDeleted = true;
            lesson.DeletedAt = now;
            lesson.DeletedBy = userName;

            lesson.UpdatedAt = now;
            lesson.UpdatedBy = userName;

            var course = await unitOfWork.Courses.Query()
                .FirstOrDefaultAsync(
                    x => x.Id == lesson.Section.CourseId,
                    cancellationToken);

            if (course is null)
            {
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    "Course not found");
            }

            course.TotalLessons = await unitOfWork.Lessons.Query()
                .CountAsync(x =>
                    !x.IsDeleted &&
                    x.Section.CourseId == course.Id,
                    cancellationToken);

            course.TotalDurationInMinutes =
                await unitOfWork.Videos.Query()
                .Where(v =>
                    !v.IsDeleted &&
                    !v.Lesson.IsDeleted &&
                    v.Lesson.Section.CourseId == course.Id)
                .SumAsync(
                    v => (int?)v.Duration,
                    cancellationToken) ?? 0;

            course.UpdatedAt = now;
            course.UpdatedBy = userName;

            await unitOfWork.SaveAsync();

            return Result<int>.Success(
                lesson.Id,
                "Lesson deleted successfully");
        }
    }



}

