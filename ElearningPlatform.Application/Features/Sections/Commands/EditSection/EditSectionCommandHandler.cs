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

namespace ElearningPlatform.Application.Features.Sections.Commands.EditSection
{
    public class EditSectionCommandHandler : IRequestHandler<EditSectionCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public EditSectionCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(EditSectionCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            var section = await unitOfWork.Sections.Query()
                .Include(c => c.Course)
                .ThenInclude(x => x.Instructor)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == request.Id
                && !x.IsDeleted, cancellationToken);
            if(section is null)
                return Result<int>.Failure(ResultStatus.NotFound,"Section not found");
            if (section.Course.Instructor.UserId != userId)
                return Result<int>.Failure(ResultStatus.Unauthorized, "You are not the instructor of this course");
            var exists = await unitOfWork.Sections.Query()
              .AnyAsync(x =>
                  x.CourseId == section.CourseId &&
                  x.Title.ToLower() == request.Title.ToLower() &&
                  x.Id != request.Id &&
                  !x.IsDeleted,
                  cancellationToken);
            if (exists)
                return Result<int>.Failure(ResultStatus.Conflict, "A section with the same title already exists in this course");

            section.Title = request.Title.Trim();
            section.Description = request.Description?.Trim();
            section.UpdatedAt = DateTime.Now;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(section.Id);
        }
    }
}
