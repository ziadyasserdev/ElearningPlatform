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

namespace ElearningPlatform.Application.Features.Sections.Commands.DeleteSection
{
    public class DeleteSectionCommandHandler : IRequestHandler<DeleteSectionCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public DeleteSectionCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(DeleteSectionCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            var section = await unitOfWork.Sections.Query()
                .Include(c => c.Course)
                .ThenInclude(x => x.Instructor)
                .ThenInclude(x => x.User)
                .Include(x => x.Lessons)
                .FirstOrDefaultAsync(x => x.Id == request.Id
                && !x.IsDeleted, cancellationToken);
            if(section is null)
                return Result<int>.Failure(ResultStatus.NotFound,"Section not found");
            if (section.Course.Instructor.UserId != userId)
                return Result<int>.Failure(ResultStatus.Unauthorized, "You are not the instructor of this course");
           if(section.Lessons.Any(x => !x.IsDeleted))
                return Result<int>.Failure(ResultStatus.Failure, "Cannot delete section with existing lessons. Please delete the lessons first.");
            section.IsDeleted = true;
            section.IsActive = false;
            section.DeletedAt = DateTime.UtcNow;
            section.DeletedBy = currentUserService.UserName;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(section.Id);

        }
    }
}
