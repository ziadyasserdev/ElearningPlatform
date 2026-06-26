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

namespace ElearningPlatform.Application.Features.Sections.Commands.CreateSection
{
    public class CreateSectionCommandHandler : IRequestHandler<CreateSectionCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public CreateSectionCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(CreateSectionCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            var course = await unitOfWork.Courses.Query()
                .Include(x => x.Instructor)
                .ThenInclude(x => x.User)
                 .FirstOrDefaultAsync(x => x.Id == request.CourseId
                 && !x.IsDeleted && x.IsActive,cancellationToken);

            if (course is null)
                return Result<int>.Failure(ResultStatus.NotFound,"Course not found");
            if (course.Instructor.UserId != userId)
                return Result<int>.Failure(ResultStatus.Forbidden, "You are not the instructor of this course");
            var exists = await unitOfWork.Sections.Query()
      .AnyAsync(x => x.CourseId == request.CourseId
          && x.Title.ToLower() == request.Title.ToLower()
          && !x.IsDeleted, cancellationToken);
            if (exists)
             return Result<int>.Failure(ResultStatus.Failure, "A section with the same title already exists in this course");
            var maxOrder = await unitOfWork.Sections.Query()
       .Where(x => x.CourseId == request.CourseId && !x.IsDeleted)
       .Select(x => (int?)x.OrderIndex)
       .MaxAsync(cancellationToken) ?? 0;
            var section = new Section
            {
                CourseId = request.CourseId,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUserService.UserName,
                Title = request.Title,
                OrderIndex = maxOrder + 1   
            };

            await unitOfWork.Sections.AddAsync(section);
            await unitOfWork.SaveAsync();
            return Result<int>.Success(section.Id);
        }
    }
}
