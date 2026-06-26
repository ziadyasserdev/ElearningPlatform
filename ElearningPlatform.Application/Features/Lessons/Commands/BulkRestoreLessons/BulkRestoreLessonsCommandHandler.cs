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

namespace ElearningPlatform.Application.Features.Lessons.Commands.BulkRestoreLessons
{
    public class BulkRestoreLessonsCommandHandler
      : IRequestHandler<BulkRestoreLessonsCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public BulkRestoreLessonsCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            BulkRestoreLessonsCommand request,
            CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            if (request.LessonIds == null || !request.LessonIds.Any())
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "No lessons provided");
            }

            var lessons = await unitOfWork.Lessons.Query()
                .Include(l => l.Section)
                    .ThenInclude(s => s.Course)
                        .ThenInclude(c => c.Instructor)
                .Where(l =>
                    request.LessonIds.Contains(l.Id) &&
                    l.IsDeleted)
                .ToListAsync(cancellationToken);

            if (!lessons.Any())
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "No deleted lessons found");
            }

            if (lessons.Any(l =>
                l.Section.Course.Instructor.UserId != userId))
            {
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "Not allowed");
            }

            var now = DateTime.UtcNow;
            var userName = currentUserService.UserName;

            var courseIds = lessons
                .Select(x => x.Section.CourseId)
                .Distinct()
                .ToList();

            foreach (var lesson in lessons)
            {
                lesson.IsDeleted = false;
                lesson.DeletedAt = null;
                lesson.DeletedBy = null;

                lesson.UpdatedAt = now;
                lesson.UpdatedBy = userName;
            }

            foreach (var courseId in courseIds)
            {
                var course = await unitOfWork.Courses.Query()
                    .FirstOrDefaultAsync(
                        x => x.Id == courseId,
                        cancellationToken);

                if (course == null)
                    continue;

                course.TotalLessons = await unitOfWork.Lessons.Query()
                    .CountAsync(x =>
                        !x.IsDeleted &&
                        x.Section.CourseId == courseId,
                        cancellationToken);

                course.TotalDurationInMinutes =
                    await unitOfWork.Videos.Query()
                    .Where(v =>
                        !v.IsDeleted &&
                        !v.Lesson.IsDeleted &&
                        v.Lesson.Section.CourseId == courseId)
                    .SumAsync(
                        v => (int?)v.Duration,
                        cancellationToken) ?? 0;

                course.UpdatedAt = now;
                course.UpdatedBy = userName;
            }

            await unitOfWork.SaveAsync();

            return Result<string>.Success(
                $"{lessons.Count} lessons restored successfully");
        }
    }




    //public class BulkRestoreLessonsCommandHandler : IRequestHandler<BulkRestoreLessonsCommand, Result<string>>
    //{
    //    private readonly IUnitOfWork unitOfWork;
    //    private readonly ICurrentUserService currentUserService;

    //    public BulkRestoreLessonsCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
    //    {
    //        this.unitOfWork = unitOfWork;
    //        this.currentUserService = currentUserService;
    //    }
    //    public async Task<Result<string>> Handle(BulkRestoreLessonsCommand request, CancellationToken cancellationToken)
    //    {
    //        var userId = currentUserService.UserId;

    //        if (request.LessonIds == null || !request.LessonIds.Any())
    //            return Result<string>.Failure(ResultStatus.Failure, "No lessons provided");

    //        var lessonsQuery = unitOfWork.Lessons.Query()
    //            .Include(l => l.Section)
    //            .ThenInclude(s => s.Course)
    //            .ThenInclude(c => c.Instructor)
    //            .Where(l => request.LessonIds.Contains(l.Id) && l.IsDeleted);

    //        var lessons = await lessonsQuery.ToListAsync(cancellationToken);

    //        if (!lessons.Any())
    //            return Result<string>.Failure(ResultStatus.NotFound, "No deleted lessons found");

    //        if (lessons.Any(l => l.Section.Course.Instructor.UserId != userId))
    //            return Result<string>.Failure(ResultStatus.Unauthorized, "Not allowed");

    //        var restoredCount = await lessonsQuery.ExecuteUpdateAsync(s => s
    //            .SetProperty(x => x.IsDeleted, false)
    //            .SetProperty(x => x.DeletedAt, (DateTime?)null)
    //            .SetProperty(x => x.DeletedBy, (string?)null)
    //            .SetProperty(x => x.UpdatedAt, DateTime.UtcNow)
    //            .SetProperty(x => x.UpdatedBy, currentUserService.UserName),
    //            cancellationToken);

    //        return Result<string>.Success($"{restoredCount} lessons restored successfully");
    //    }
    //}
}
