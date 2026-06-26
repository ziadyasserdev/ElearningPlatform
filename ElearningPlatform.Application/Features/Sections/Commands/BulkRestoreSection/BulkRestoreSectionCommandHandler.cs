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

namespace ElearningPlatform.Application.Features.Sections.Commands.BulkRestoreSection
{
    public class BulkRestoreSectionCommandHandler : IRequestHandler<BulkRestoreSectionCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public BulkRestoreSectionCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(BulkRestoreSectionCommand request, CancellationToken cancellationToken)
        {

            var sectionIds = request.SectionIds.Distinct().ToList();

           
            var sectionsToRestore = await unitOfWork.Sections.Query()
                .Where(x => sectionIds.Contains(x.Id) && x.IsDeleted)
                .ToListAsync(cancellationToken);

            if (!sectionsToRestore.Any())
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "No sections found to restore"
                );

          
            var sectionNames = sectionsToRestore
                .Select(x => x.Title.Trim().ToLower())
                .ToList();

           
            var conflictingNames = await unitOfWork.Sections.Query()
                .Where(x => !x.IsDeleted && sectionNames.Contains(x.Title.Trim().ToLower()))
                .Select(x => x.Title.Trim().ToLower())
                .ToListAsync(cancellationToken);

        
            var sectionsAllowedToRestore = sectionsToRestore
                .Where(x => !conflictingNames.Contains(x.Title.Trim().ToLower()))
                .ToList();

            if (!sectionsAllowedToRestore.Any())
            {
                return Result<string>.Success(
                    "No sections were restored due to name conflicts"
                );
            }

          
            await unitOfWork.Sections.Query()
                .Where(x => sectionsAllowedToRestore.Select(s => s.Id).Contains(x.Id))
                .ExecuteUpdateAsync(x => x
                    .SetProperty(s => s.IsDeleted, false)
                    .SetProperty(s => s.DeletedAt, (DateTime?)null)
                    .SetProperty(s => s.DeletedBy, (string?)null)
                    .SetProperty(s => s.UpdatedAt, DateTime.UtcNow)
                    .SetProperty(s => s.UpdatedBy, currentUserService.UserName),
                    cancellationToken);

      
            return Result<string>.Success(
                $"{sectionsAllowedToRestore.Count} sections restored successfully. " +
                $"{sectionsToRestore.Count - sectionsAllowedToRestore.Count} skipped due to name conflicts."
            );

        }
    }
}
