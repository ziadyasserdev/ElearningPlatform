using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<int>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var name = request.Name.Trim();

           
            var exists = await unitOfWork.Categories.Query()
                .AnyAsync(c => c.Name == name
                && !c.IsDeleted
                , cancellationToken);

            if (exists)
                return Result<int>.Failure(ResultStatus.Conflict, "Category name already exists.");

           
            var slug = Regex.Replace(name.ToLower(), @"[^a-z0-9\s-]", "");
            slug = Regex.Replace(slug, @"\s+", "-");

            var baseSlug = slug;
            int counter = 1;

            while (await unitOfWork.Categories.Query()
                .AnyAsync(c => c.Slug == slug, cancellationToken))
            {
                slug = $"{baseSlug}-{counter++}";
            }

            var category = new Category
            {
                Name = name,
                Description = request.Description!,
                Slug = slug,
                IsActive = true,
                CreatedAt = DateTime.Now,
                CreatedBy = currentUserService.UserName
            };

            await unitOfWork.Categories.AddAsync(category);
            await unitOfWork.SaveAsync();

            return Result<int>.Success(category.Id);
        }
    }
}
