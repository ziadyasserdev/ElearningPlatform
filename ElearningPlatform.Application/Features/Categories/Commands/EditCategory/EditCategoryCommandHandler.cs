using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Commands.EditCategory
{
    public class EditCategoryCommandHandler : IRequestHandler<EditCategoryCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public EditCategoryCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.Categories.Query()
         .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (category is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Category not found.");

         
            var newName = request.Name?.Trim();

       
            if (!string.IsNullOrWhiteSpace(newName) && newName != category.Name)
            {
                var exists = await unitOfWork.Categories.Query()
                    .AnyAsync(x => x.Name == newName 
                    && x.Id != request.Id
                    && !x.IsDeleted
                    , cancellationToken);

                if (exists)
                    return Result<int>.Failure(ResultStatus.Conflict, "Category name already exists.");

            
                var slug = Regex.Replace(newName.ToLower(), @"[^a-z0-9\s-]", "");
                slug = Regex.Replace(slug, @"\s+", "-");

                var baseSlug = slug;
                int counter = 1;

                while (await unitOfWork.Categories.Query()
                    .AnyAsync(x => x.Slug == slug && x.Id != request.Id, cancellationToken))
                {
                    slug = $"{baseSlug}-{counter++}";
                }

                category.Name = newName;
                category.Slug = slug;
            }

         
            if (request.Description is not null)
                category.Description = request.Description.Trim();

           category.UpdatedAt = DateTime.Now;
            category.UpdatedBy = currentUserService.UserName;

            try
            {
                await unitOfWork.SaveAsync();
            }
            catch (DbUpdateException)
            {
                return Result<int>.Failure(ResultStatus.Conflict, "Category already exists.");
            }

            return Result<int>.Success(category.Id);
        }
    }
}
