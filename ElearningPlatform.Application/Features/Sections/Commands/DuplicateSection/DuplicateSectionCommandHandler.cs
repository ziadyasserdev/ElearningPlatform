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

namespace ElearningPlatform.Application.Features.Sections.Commands.DuplicateSection
{
    public class DuplicateSectionCommandHandler : IRequestHandler<DuplicateSectionCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public DuplicateSectionCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(DuplicateSectionCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            var section = await unitOfWork.Sections.Query()
   .Include(x => x.Course)
   .ThenInclude(x => x.Instructor)
   .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

            if (section is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Section not found");

            if (section.Course.Instructor.UserId != userId &&
                !currentUserService.IsInRole("Admin"))
            {
                return Result<int>.Failure(ResultStatus.Unauthorized, "Not allowed");
            }

            var lessons = await unitOfWork.Lessons.Query()
                .Where(x => x.SectionId == section.Id && !x.IsDeleted)
                .ToListAsync(cancellationToken);

        

            var maxOrder = await unitOfWork.Sections.Query()
                .Where(x => x.CourseId == section.CourseId && !x.IsDeleted)
                .MaxAsync(x => (int?)x.OrderIndex) ?? 0;

            var newSection = new Section
            {
                Title = section.Title + " (Copy)",
                Description = section.Description,
                CourseId = section.CourseId,
                OrderIndex = maxOrder + 1,
                CreatedAt = DateTime.Now,
                CreatedBy = currentUserService.UserName,
                Lessons = lessons.Select(l => new Lesson
                {
                    Title = l.Title + " (Copy)",
                    Description = l.Description,
                    Duration = l.Duration,
                    OrderIndex = l.OrderIndex,
                    IsPreview = l.IsPreview,
                    IsPublished = false,
                    CreatedAt = DateTime.Now,
                    CreatedBy = currentUserService.UserName
                }).ToList()
            };

            await unitOfWork.Sections.AddAsync(newSection);
            await unitOfWork.SaveAsync();

            return Result<int>.Success(newSection.Id);
        }
    }
}
