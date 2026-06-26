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

namespace ElearningPlatform.Application.Features.Lessons.Commands.ToggleLessonPreview
{
    public class ToggleLessonPreviewCommandHandler : IRequestHandler<ToggleLessonPreviewCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public ToggleLessonPreviewCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(ToggleLessonPreviewCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var lesson = await unitOfWork.Lessons.Query()
                .Include(l => l.Section)
                .ThenInclude(s => s.Course)
                .ThenInclude(c => c.Instructor)
                .FirstOrDefaultAsync(l => l.Id == request.LessonId && !l.IsDeleted, cancellationToken);

            if (lesson is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Lesson not found");

          
            if (lesson.Section.Course.Instructor.UserId != userId)
                return Result<int>.Failure(ResultStatus.Unauthorized, "Not allowed");

           if(!lesson.IsPublished)
                return Result<int>.Failure(ResultStatus.Failure, "Only published lessons can be set as preview");
            lesson.IsPreview = !lesson.IsPreview;

            lesson.UpdatedAt = DateTime.UtcNow;
            lesson.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<int>.Success(lesson.Id, $"Preview {(lesson.IsPreview ? "enabled" : "disabled")}");
        }
    }
}
