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

namespace ElearningPlatform.Application.Features.Sections.Commands.BulkUpdateSectionStatus
{
    public class BulkUpdateSectionStatusCommandHandler : IRequestHandler<BulkUpdateSectionStatusCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public BulkUpdateSectionStatusCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(BulkUpdateSectionStatusCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            if (request.SectionIds == null || !request.SectionIds.Any())
                return Result<string>.Failure(ResultStatus.Failure, "No sections provided");

            
            var sectionsQuery = unitOfWork.Sections.Query()
                .Where(x => request.SectionIds.Contains(x.Id) && !x.IsDeleted);


          
            var sections = await sectionsQuery
                .Select(x => new
                {
                    x.Id,
                    x.IsActive
                })
                .ToListAsync(cancellationToken);

            if (!sections.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "No sections found");

           
            var idsToUpdate = sections
                .Where(x => x.IsActive != request.IsActive)
                .Select(x => x.Id)
                .ToList();

            if (!idsToUpdate.Any())
            {
                return Result<string>.Success(
                    "No changes applied (all sections already in desired state)"
                );
            }

          
            var affectedRows = await unitOfWork.Sections.Query()
                .Where(x => idsToUpdate.Contains(x.Id))
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.IsActive, request.IsActive)
                    .SetProperty(x => x.UpdatedAt, DateTime.UtcNow)
                    .SetProperty(x => x.UpdatedBy, currentUserService.UserName),
                    cancellationToken);

           
            var action = request.IsActive ? "activated" : "deactivated";

            return Result<string>.Success(
                $"{affectedRows} sections {action} successfully"
            );
        }
    }
    }

