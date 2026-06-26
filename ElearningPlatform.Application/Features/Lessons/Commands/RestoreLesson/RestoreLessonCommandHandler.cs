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

namespace ElearningPlatform.Application.Features.Lessons.Commands.RestoreLesson
{
    public class RestoreLessonCommandHandler
        : IRequestHandler<RestoreLessonCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public RestoreLessonCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(
            RestoreLessonCommand request,
            CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var lessonInfo = await unitOfWork.Lessons.Query()
                .Where(x => x.Id == request.Id && x.IsDeleted)
                .Select(x => new
                {
                    Lesson = x,
                    SectionId = x.SectionId,
                    CourseId = x.Section.CourseId,
                    InstructorId = x.Section.Course.Instructor.UserId
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (lessonInfo is null)
            {
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    "Lesson not found");
            }

            if (lessonInfo.InstructorId != userId)
            {
                return Result<int>.Failure(
                    ResultStatus.Unauthorized,
                    "You are not the instructor of this course");
            }

            var checkDuplicate = await unitOfWork.Lessons.Query()
                .AnyAsync(x =>
                    x.Title == lessonInfo.Lesson.Title &&
                    x.SectionId == lessonInfo.SectionId &&
                    !x.IsDeleted,
                    cancellationToken);

            if (checkDuplicate)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "A lesson with the same title already exists in this section");
            }

            var maxOrder = await unitOfWork.Lessons.Query()
                .Where(x =>
                    x.SectionId == lessonInfo.SectionId &&
                    !x.IsDeleted)
                .MaxAsync(x => (int?)x.OrderIndex, cancellationToken) ?? 0;

            lessonInfo.Lesson.IsDeleted = false;
            lessonInfo.Lesson.DeletedAt = null;
            lessonInfo.Lesson.DeletedBy = null;

            lessonInfo.Lesson.OrderIndex = maxOrder + 1;

            lessonInfo.Lesson.UpdatedAt = DateTime.UtcNow;
            lessonInfo.Lesson.UpdatedBy = currentUserService.UserName;

            var course = await unitOfWork.Courses.Query()
                .FirstOrDefaultAsync(
                    x => x.Id == lessonInfo.CourseId,
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

            course.UpdatedAt = DateTime.Now;
            course.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<int>.Success(
                lessonInfo.Lesson.Id,
                "Lesson restored successfully");
        }
    }
    //public class RestoreLessonCommandHandler : IRequestHandler<RestoreLessonCommand, Result<int>>
    //{
    //    private readonly IUnitOfWork unitOfWork;
    //    private readonly ICurrentUserService currentUserService;

    //    public RestoreLessonCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
    //    {
    //        this.unitOfWork = unitOfWork;
    //        this.currentUserService = currentUserService;
    //    }
    //    public async Task<Result<int>> Handle(RestoreLessonCommand request, CancellationToken cancellationToken)
    //    {
    //        var userId = currentUserService.UserId;
    //        var LessonInfo = await unitOfWork.Lessons.Query()
    //           .Where(x => x.Id == request.Id
    //            && x.IsDeleted)
    //           .Select(x => new
    //           {
    //               Lesson = x,
    //               SectionId = x.SectionId,
    //               InstructorId = x.Section.Course.Instructor.UserId

    //           }).FirstOrDefaultAsync(cancellationToken);

    //        if(LessonInfo is null)
    //            return Result<int>.Failure(ResultStatus.NotFound, "Lesson not found");
    //        if(LessonInfo.InstructorId != userId)
    //            return Result<int>.Failure(ResultStatus.Unauthorized, "You are not the instructor of this course");

    //        var checkDuplicate = await unitOfWork.Lessons.Query()
    //            .AnyAsync(x => EF.Functions.Like(x.Title, LessonInfo.Lesson.Title)
    //            && x.SectionId == LessonInfo.SectionId
    //            && !x.IsDeleted, cancellationToken);

    //        if(checkDuplicate)
    //            return Result<int>.Failure(ResultStatus.Conflict, "A lesson with the same title already exists in this section. Please choose a different title.");

    //        var MaxOrder = await unitOfWork.Lessons.Query()
    //            .Where(x => x.SectionId == LessonInfo.SectionId && !x.IsDeleted)
    //            .MaxAsync(x => (int?)x.OrderIndex) ?? 0;
    //        LessonInfo.Lesson.IsDeleted = false;
    //        LessonInfo.Lesson.DeletedAt = null;
    //        LessonInfo.Lesson.DeletedBy = null;
    //        LessonInfo.Lesson.OrderIndex = MaxOrder + 1;
    //        LessonInfo.Lesson.UpdatedAt = DateTime.UtcNow;
    //        LessonInfo.Lesson.UpdatedBy = currentUserService.UserName;

    //        var course = await unitOfWork.Sections.Query()
    //            .Include(x => x.Course)
    //            .Where(x => x.Id == LessonInfo.SectionId && !x.IsDeleted)
    //            .Select(x => x.Course).FirstOrDefaultAsync(cancellationToken);
    //        if(course == null)
    //            return Result<int>.Failure(ResultStatus.NotFound, "Course not found");
    //        course.TotalLessons += 1;
    //        await unitOfWork.SaveAsync();
    //        return Result<int>.Success(LessonInfo.Lesson.Id);
    //    }
    //}
}
