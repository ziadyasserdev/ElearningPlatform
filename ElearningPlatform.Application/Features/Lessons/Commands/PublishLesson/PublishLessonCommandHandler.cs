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

namespace ElearningPlatform.Application.Features.Lessons.Commands.PublishLesson
{
    //public class PublishLessonCommandHandler : IRequestHandler<PublishLessonCommand, Result<int>>
    //{
    //    private readonly IUnitOfWork unitOfWork;
    //    private readonly ICurrentUserService currentUserService;

    //    public PublishLessonCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    //    {
    //        this.unitOfWork = unitOfWork;
    //        this.currentUserService = currentUserService;
    //    }
    //    public async Task<Result<int>> Handle(PublishLessonCommand request, CancellationToken cancellationToken)
    //    {
    //        var userId = currentUserService.UserId;

    //        var lessonData = await unitOfWork.Lessons.Query()
    //            .Where(l => l.Id == request.Id && !l.IsDeleted)
    //            .Select(l => new
    //            {
    //                Lesson = l,
    //                InstructorUserId = l.Section.Course.Instructor.UserId,
    //                HasVideos = l.Videos.Any(v => !v.IsDeleted)
    //            })
    //            .FirstOrDefaultAsync(cancellationToken);

    //        if (lessonData == null)
    //            return Result<int>.Failure(ResultStatus.NotFound, "Lesson not found");

    //        if (lessonData.InstructorUserId != userId)
    //            return Result<int>.Failure(ResultStatus.Unauthorized, "Not allowed");

    //        if (!lessonData.HasVideos)
    //            return Result<int>.Failure(ResultStatus.Failure, "Cannot publish lesson without videos");

    //        if (lessonData.Lesson.IsPublished)
    //            return Result<int>.Failure(ResultStatus.Conflict, "Lesson already published");


    //        lessonData.Lesson.IsPublished = true;
    //        lessonData.Lesson.UpdatedAt = DateTime.Now;
    //        lessonData.Lesson.UpdatedBy = currentUserService.UserName;

    //        await unitOfWork.SaveAsync();

    //        return Result<int>.Success(lessonData.Lesson.Id, "Lesson published successfully");
    //    }
    //}
    public class PublishLessonCommandHandler
      : IRequestHandler<PublishLessonCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public PublishLessonCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(
            PublishLessonCommand request,
            CancellationToken cancellationToken)
        {
            var lesson = await unitOfWork.Lessons.Query()
                .Include(l => l.Videos)
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

            if (lesson.Section.Course.Instructor.UserId != currentUserService.UserId)
            {
                return Result<int>.Failure(
                    ResultStatus.Forbidden,
                    "You are not the instructor of this course");
            }

            if (lesson.IsPublished)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "Lesson already published");
            }

            var hasVideos = lesson.Videos
                .Any(v => !v.IsDeleted);

            if (!hasVideos)
            {
                return Result<int>.Failure(
                    ResultStatus.Failure,
                    "Lesson must contain at least one video");
            }

            lesson.IsPublished = true;

            lesson.UpdatedAt = DateTime.Now;
            lesson.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<int>.Success(
                lesson.Id,
                "Lesson published successfully");
        }
    }
}
