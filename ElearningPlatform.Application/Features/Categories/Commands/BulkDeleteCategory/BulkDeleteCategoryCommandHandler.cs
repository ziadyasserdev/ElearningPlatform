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

namespace ElearningPlatform.Application.Features.Categories.Commands.BulkDeleteCategory
{
    public class BulkDeleteCategoryCommandHandler:IRequestHandler<BulkDeleteCategoryCommand,Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public BulkDeleteCategoryCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(BulkDeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryIds = request.CategoryIds.Distinct().ToList();

            var categories = await unitOfWork.Categories.Query()
                .Include(x => x.Courses)
                .Where(x => categoryIds.Contains(x.Id) && !x.IsDeleted)
                .ToListAsync(cancellationToken);

            if (!categories.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "No valid categories found to delete.");

            var categoriesWithCourses = categories
                .Where(c => c.Courses.Any(x => !x.IsDeleted))
                .Select(c => c.Name)
                .ToList();

            if (categoriesWithCourses.Any())
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    $"Cannot delete categories because they contain courses: {string.Join(", ", categoriesWithCourses)}"
                );

            foreach (var category in categories)
            {
                category.IsDeleted = true;
                category.IsActive = false;
                category.DeletedAt = DateTime.UtcNow;
                category.DeletedBy = currentUserService.UserName;
            }

            await unitOfWork.SaveAsync();

            return Result<string>.Success($"{categories.Count} categories deleted successfully.");
        }
    }
}
