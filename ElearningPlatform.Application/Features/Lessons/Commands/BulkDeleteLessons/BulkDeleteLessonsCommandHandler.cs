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

namespace ElearningPlatform.Application.Features.Lessons.Commands.BulkDeleteLessons
{
    public class BulkDeleteLessonsCommandHandler
      : IRequestHandler<BulkDeleteLessonsCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public BulkDeleteLessonsCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            BulkDeleteLessonsCommand request,
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
                    !l.IsDeleted)
                .ToListAsync(cancellationToken);

            if (!lessons.Any())
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "No lessons found");
            }

            if (lessons.Any(l =>
                l.Section.Course.Instructor.UserId != userId))
            {
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "You are not allowed to delete these lessons");
            }

            var courseIds = lessons
                .Select(x => x.Section.CourseId)
                .Distinct()
                .ToList();

            foreach (var lesson in lessons)
            {
                lesson.IsDeleted = true;
                lesson.DeletedAt = DateTime.UtcNow;
                lesson.DeletedBy = currentUserService.UserName;
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

                course.UpdatedAt = DateTime.UtcNow;
                course.UpdatedBy = currentUserService.UserName;
            }

            await unitOfWork.SaveAsync();

            return Result<string>.Success(
                $"{lessons.Count} lessons deleted successfully");
        }
    }
    //public class BulkDeleteLessonsCommandHandler:IRequestHandler<BulkDeleteLessonsCommand, Result<string>>
    //{
    //    private readonly IUnitOfWork unitOfWork;
    //    private readonly ICurrentUserService currentUserService;

    //    public BulkDeleteLessonsCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
    //    {
    //        this.unitOfWork = unitOfWork;
    //        this.currentUserService = currentUserService;
    //    }

    //    public async Task<Result<string>> Handle(BulkDeleteLessonsCommand request, CancellationToken cancellationToken)
    //    {
    //        var userId = currentUserService.UserId;

    //        if (request.LessonIds == null || !request.LessonIds.Any())
    //            return Result<string>.Failure(ResultStatus.Failure, "No lessons provided");

    //        var lessonsQuery = unitOfWork.Lessons.Query()
    //            .Include(l => l.Section)
    //            .ThenInclude(s => s.Course)
    //            .ThenInclude(c => c.Instructor)
    //            .Where(l => request.LessonIds.Contains(l.Id) && !l.IsDeleted);

    //        var lessons = await lessonsQuery.ToListAsync(cancellationToken);

    //        if (!lessons.Any())
    //            return Result<string>.Failure(ResultStatus.NotFound, "No lessons found");

    //        if (lessons.Any(l => l.Section.Course.Instructor.UserId != userId))
    //            return Result<string>.Failure(ResultStatus.Unauthorized, "Not allowed");

    //        var deletedCount = await lessonsQuery.ExecuteUpdateAsync(s => s
    //            .SetProperty(x => x.IsDeleted, true)
    //            .SetProperty(x => x.DeletedAt, DateTime.Now)
    //            .SetProperty(x => x.DeletedBy, currentUserService.UserName),
    //            cancellationToken);





    //        return Result<string>.Success($"{deletedCount} lessons deleted successfully");
    //    }
    //}
}
