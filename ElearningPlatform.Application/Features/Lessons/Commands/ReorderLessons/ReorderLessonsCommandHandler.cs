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

namespace ElearningPlatform.Application.Features.Lessons.Commands.ReorderLessons
{
    public class ReorderLessonsCommandHandler : IRequestHandler<ReorderLessonsCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public ReorderLessonsCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService   currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(ReorderLessonsCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

          
            var sectionInfo = await unitOfWork.Sections.Query()
                .Where(x => x.Id == request.SectionId && !x.IsDeleted && x.IsActive)
                .Select(x => new
                {
                    x.Id,
                    InstructorId = x.Course.Instructor.UserId
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (sectionInfo is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Section not found");

            if (sectionInfo.InstructorId != userId)
                return Result<string>.Failure(ResultStatus.Unauthorized, "Not allowed");

          
            if (request.LessonIds.Count != request.LessonIds.Distinct().Count())
                return Result<string>.Failure(ResultStatus.Failure, "Duplicate lesson IDs are not allowed");

          
            var lessons = await unitOfWork.Lessons.Query()
                .Where(x => x.SectionId == request.SectionId && !x.IsDeleted)
                .ToListAsync(cancellationToken);

            if (!lessons.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "Lessons not found");

           
            if (lessons.Count != request.LessonIds.Count)
                return Result<string>.Failure(ResultStatus.Failure, "Invalid lessons list");

            
            var lessonsDict = lessons.ToDictionary(x => x.Id);

            int order = 1;

            foreach (var id in request.LessonIds)
            {
                if (!lessonsDict.TryGetValue(id, out var lesson))
                    return Result<string>.Failure(ResultStatus.NotFound, $"Lesson with id {id} not found");

                lesson.OrderIndex = order++;
                lesson.UpdatedAt = DateTime.UtcNow;
                lesson.UpdatedBy = currentUserService.UserName;
            }

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Lessons reordered successfully");
        }
    }
}
