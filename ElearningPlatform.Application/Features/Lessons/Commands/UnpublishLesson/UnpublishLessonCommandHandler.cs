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

namespace ElearningPlatform.Application.Features.Lessons.Commands.UnpublishLesson
{
    public class UnpublishLessonCommandHandler : IRequestHandler<UnpublishLessonCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public UnpublishLessonCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(UnpublishLessonCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var lessonData = await unitOfWork.Lessons.Query()
                .Where(l => l.Id == request.Id && !l.IsDeleted)
                .Select(l => new
                {
                    Lesson = l,
                    InstructorUserId = l.Section.Course.Instructor.UserId,
                    HasProgress = l.Videos.SelectMany(v => v.VideoProgresses).Any() 
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (lessonData == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Lesson not found");

            if (lessonData.InstructorUserId != userId)
                return Result<int>.Failure(ResultStatus.Unauthorized, "Not allowed");
           
            if (!lessonData.Lesson.IsPublished)
                return Result<int>.Failure(ResultStatus.Conflict, "Lesson is already unpublished");

          if(lessonData.HasProgress)
                return Result<int>.Failure(ResultStatus.Failure, "Cannot unpublish lesson with student progress");


            lessonData.Lesson.IsPublished = false;
            lessonData.Lesson.UpdatedAt = DateTime.UtcNow;
            lessonData.Lesson.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();
            return Result<int>.Success(lessonData.Lesson.Id, "Lesson unpublished successfully");
        }
    }
}
