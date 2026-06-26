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

namespace ElearningPlatform.Application.Features.Lessons.Commands.EditLesson
{
    public class EditLessonCommandHandler : IRequestHandler<EditLessonCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public EditLessonCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(EditLessonCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

          
            var lessonData = await unitOfWork.Lessons.Query()
                .Where(l => l.Id == request.Id && !l.IsDeleted)
                .Select(l => new
                {
                    Lesson = l,
                    l.SectionId,
                    InstructorUserId = l.Section.Course.Instructor.UserId
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (lessonData == null)
                return Result<int>.Failure(ResultStatus.NotFound, "Lesson not found");

            if (lessonData.InstructorUserId != userId)
                return Result<int>.Failure(ResultStatus.Unauthorized, "Not allowed");

          
            var exists = await unitOfWork.Lessons.Query()
                .AnyAsync(l =>
                    l.SectionId == lessonData.SectionId &&
                    l.Title.ToLower() == request.Title.ToLower() &&
                    l.Id != request.Id &&
                    !l.IsDeleted,
                    cancellationToken);

            if (exists)
                return Result<int>.Failure(ResultStatus.Conflict, "Lesson title already exists");

       
            lessonData.Lesson.Title = request.Title;
            lessonData.Lesson.Description = request.Description;
            lessonData.Lesson.UpdatedAt = DateTime.UtcNow;
            lessonData.Lesson.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<int>.Success(lessonData.Lesson.Id);
        }
    }
}
