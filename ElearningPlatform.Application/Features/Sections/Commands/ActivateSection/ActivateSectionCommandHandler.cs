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

namespace ElearningPlatform.Application.Features.Sections.Commands.ActivateSection
{
    public class ActivateSectionCommandHandler : IRequestHandler<ActivateSectionCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public ActivateSectionCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(ActivateSectionCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            var section = await unitOfWork.Sections.Query()
                .Include(x => x.Course)
                .ThenInclude(x => x.Instructor)
             
                 .FirstOrDefaultAsync(x => x.Id == request.Id 
                 && !x.IsDeleted,cancellationToken);
            if(section is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Section not found");
   if(section.IsActive)
                return Result<int>.Failure(ResultStatus.Failure, "Section is already active.");
            if (section.Course.Instructor.UserId != userId)
                return Result<int>.Failure(ResultStatus.Unauthorized, "You are not authorized to activate this section.");
            if(!section.Course.IsActive)
                return Result<int>.Failure(ResultStatus.Failure, "Cannot activate a section of an inactive course. Please activate the course first.");
            section.IsActive = true;
            section.UpdatedAt = DateTime.Now;
            section.UpdatedBy = currentUserService.UserName;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(section.Id);

        }
    }
}
