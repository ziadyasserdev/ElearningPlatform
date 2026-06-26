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

namespace ElearningPlatform.Application.Features.Categories.Commands.BulkRestoreCategory
{
    public class BulkRestoreCategoryCommandHandler : IRequestHandler<BulkRestoreCategoryCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public BulkRestoreCategoryCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(BulkRestoreCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryIds = request.CategoryIds.Distinct().ToList();

            var categories = await unitOfWork.Categories.Query()
                .Where(x => categoryIds.Contains(x.Id) && x.IsDeleted)
                .ToListAsync(cancellationToken);

            if (!categories.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "No categories found.");

            var categoryNames = categories.Select(c => c.Name).ToList();

            var existingNames = await unitOfWork.Categories.Query()
                .Where(x => categoryNames.Contains(x.Name) && !x.IsDeleted)
                .Select(x => x.Name)
                .ToListAsync(cancellationToken);

            if (existingNames.Any())
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    $"Restore failed due to existing names: {string.Join(", ", existingNames)}"
                );

            foreach (var category in categories)
            {
                category.IsDeleted = false;
                category.DeletedAt = null;
                category.DeletedBy = null;

                

                category.UpdatedAt = DateTime.UtcNow;
                category.UpdatedBy = currentUserService.UserName;
            }

            await unitOfWork.SaveAsync();

            return Result<string>.Success(
                $"{categories.Count} categories restored successfully."
            );
        }
    }
}
    