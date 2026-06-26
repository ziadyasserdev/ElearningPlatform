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

namespace ElearningPlatform.Application.Features.Sections.Commands.DeactivateSection
{
    public class DeactivateSectionCommandHandler : IRequestHandler<DeactivateSectionCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public DeactivateSectionCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(DeactivateSectionCommand request, CancellationToken cancellationToken)
        {
           var section = await unitOfWork.Sections.Query()
                .Include(x => x.Course)
                .ThenInclude(x => x.Instructor)
                .FirstOrDefaultAsync(x => x.Id == request.Id
                && !x.IsDeleted,cancellationToken);

            if(section is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Section not found"); 

            if(!section.IsActive)
                return Result<int>.Failure(ResultStatus.Failure, "Section is already inactive.");
            if(section.Course.Instructor.UserId != currentUserService.UserId)
                return Result<int>.Failure(ResultStatus.Unauthorized, "You are not authorized to deactivate this section.");

            var hasLessons = await unitOfWork.Lessons.Query()
    .AnyAsync(l => l.SectionId == section.Id && !l.IsDeleted, cancellationToken);

            if (hasLessons)
                return Result<int>.Failure(ResultStatus.Failure, "Cannot deactivate section with lessons");

            section.IsActive = false;
            section.UpdatedAt = DateTime.UtcNow;
            section.UpdatedBy = currentUserService.UserName;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(section.Id);
        }
    }
}
