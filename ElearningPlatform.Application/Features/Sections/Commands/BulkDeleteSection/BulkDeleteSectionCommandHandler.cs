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

namespace ElearningPlatform.Application.Features.Sections.Commands.BulkDeleteSection
{
    public class BulkDeleteSectionCommandHandler : IRequestHandler<BulkDeleteSectionCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public BulkDeleteSectionCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(BulkDeleteSectionCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            if (request.SectionIds == null || !request.SectionIds.Any())
                return Result<string>.Failure(ResultStatus.Failure, "No sections provided");

         
            var sectionsQuery = unitOfWork.Sections.Query()
                .Where(x => request.SectionIds.Contains(x.Id) && !x.IsDeleted);

        

            var sections = await sectionsQuery
                .Include(x => x.Lessons)
                .ToListAsync(cancellationToken);

            if (!sections.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "No sections found");

         
            var blockedSections = sections
                .Where(x => x.Lessons.Any())
                .ToList();

            if (blockedSections.Any())
            {
                return Result<string>.Failure(
                    ResultStatus.Failure,
                    $"{blockedSections.Count} sections contain lessons and cannot be deleted"
                );
            }

         
            await unitOfWork.Sections.Query()
                .Where(x => sections.Select(s => s.Id).Contains(x.Id))
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.IsDeleted, true)
                    .SetProperty(x => x.DeletedAt, DateTime.Now)
                    .SetProperty(x => x.DeletedBy, currentUserService.UserName)
                    .SetProperty(x => x.IsActive, false),
                    
                    cancellationToken);

            return Result<string>.Success(
                $"{sections.Count} sections deleted successfully"
            );
        }
    }
    
}
