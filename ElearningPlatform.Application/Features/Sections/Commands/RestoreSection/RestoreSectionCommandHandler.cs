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

namespace ElearningPlatform.Application.Features.Sections.Commands.RestoreSection
{
    public class RestoreSectionCommandHandler : IRequestHandler<RestoreSectionCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public RestoreSectionCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(RestoreSectionCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            var section = await unitOfWork.Sections.Query()
                .Include(x => x.Course)
                .ThenInclude(x => x.Instructor)
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.IsDeleted, cancellationToken);

            if(section is null)
                return Result<int>.Failure(ResultStatus.NotFound,"Section not found or not deleted");
            if(section.Course.Instructor.UserId != userId)
                return Result<int>.Failure(ResultStatus.Unauthorized, "You are not the instructor of this course");
            var exist = await unitOfWork.Sections.Query()
                .AnyAsync(x => x.Title.ToLower() == section.Title.ToLower()
                && x.CourseId == section.CourseId 
                && !x.IsDeleted, cancellationToken);
            if(exist)
                return Result<int>.Failure(ResultStatus.Failure, "A section with the same title already exists in this course. Please rename the existing section before restoring this one.");
            section.IsDeleted = false;
            section.DeletedAt = null;
            section.DeletedBy = null;
            section.UpdatedAt = DateTime.Now;
            section.UpdatedBy = currentUserService.UserName;
            await unitOfWork.SaveAsync();
            return Result<int>.Success(section.Id);
        }
    }
}
