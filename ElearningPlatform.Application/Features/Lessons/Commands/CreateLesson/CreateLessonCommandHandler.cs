using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Lessons.Commands.CreateLesson
{
    public class CreateLessonCommandHandler : IRequestHandler<CreateLessonCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public CreateLessonCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(CreateLessonCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
           var section = await unitOfWork.Sections.Query()
                .Include(x => x.Course)
                .ThenInclude(x => x.Instructor)
                .FirstOrDefaultAsync(x => x.Id == request.SectionId
                && !x.IsDeleted
                && x.IsActive
                , cancellationToken);

            if(section is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Section not found");
            if(section.Course.Instructor.UserId != userId)
                return Result<int>.Failure(ResultStatus.Unauthorized, "You are not the instructor of this course");
            var checkExist = await unitOfWork.Lessons.Query()
                .AnyAsync(x => !x.IsDeleted && x.SectionId == request.SectionId && x.Title == request.Title, cancellationToken);
            if(checkExist)
                return Result<int>.Failure(ResultStatus.Conflict, "A lesson with the same title already exists in this section. Please choose a different title.");
            var maxOrder = await unitOfWork.Lessons.Query()
                .Where(x => x.SectionId == request.SectionId && !x.IsDeleted)
                .MaxAsync(x => (int?)x.OrderIndex) ?? 0;






            section.Course.TotalLessons += 1;

            var lesson = new Lesson
            {
                Title = request.Title,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                 CreatedBy = currentUserService.UserName,
                 SectionId = request.SectionId,
                 OrderIndex = maxOrder+1,
                 IsPreview = false
            };
            await unitOfWork.Lessons.AddAsync(lesson);
            await unitOfWork.SaveAsync();
            return Result<int>.Success(lesson.Id);
        }
    }
}
