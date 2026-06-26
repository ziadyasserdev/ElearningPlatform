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

namespace ElearningPlatform.Application.Features.Categories.Commands.BulkToggleCategoryStatus
{
    public class BulkToggleCategoryStatusCommandHandler : IRequestHandler<BulkToggleCategoryStatusCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public BulkToggleCategoryStatusCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(BulkToggleCategoryStatusCommand request, CancellationToken cancellationToken)
        {
            var categoryIds = request.CategoryIds.Distinct().ToList();

            var categories = await unitOfWork.Categories.Query()
                .Include(x => x.Courses)
                .Where(x => categoryIds.Contains(x.Id) && !x.IsDeleted)
                .ToListAsync(cancellationToken);

            if (!categories.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "No categories found.");

            var updated = 0;
            var skipped = 0;

            foreach (var category in categories)
            {
                bool hasActiveCourses = category.Courses.Any(x => !x.IsDeleted && x.IsActive);

                if (request.IsActive && hasActiveCourses)
                {
                    skipped++;
                    continue;
                }

                if (!request.IsActive && hasActiveCourses)
                {
                    skipped++;
                    continue;
                }

                category.IsActive = request.IsActive;
                category.UpdatedAt = DateTime.UtcNow;
                category.UpdatedBy = currentUserService.UserName;

                updated++;
            }

            await unitOfWork.SaveAsync();

            return Result<string>.Success(
                $"{updated} categories {(request.IsActive ? "activated" : "deactivated")}, {skipped} skipped."
            );
        }
    }
}
