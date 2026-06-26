using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Categories.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Result<object>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<object>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {

            var query = unitOfWork.Categories.Query();

            if (currentUserService.IsInRole("Admin"))
            {
                var category = await query
                    .Where(c => c.Id == request.Id)
                    .Select(c => new CategoryForAdminDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Slug = c.Slug,
                        Description = c.Description,
                        IconUrl = c.IconUrl,
                        IsActive = c.IsActive,
                        IsDeleted = c.IsDeleted,
                        CoursesCount = c.Courses.Count()
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                return category is null
                    ? Result<object>.Failure(ResultStatus.NotFound, "Category not found.")
                    : Result<object>.Success(category);
            }
            else
            {
                var category = await query
                    .Where(c => c.Id == request.Id && c.IsActive && !c.IsDeleted)
                    .Select(c => new CategoryForUserDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Slug = c.Slug,
                        Description = c.Description,
                        IconUrl = c.IconUrl
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                return category is null
                    ? Result<object>.Failure(ResultStatus.NotFound, "Category not found or inactive.")
                    : Result<object>.Success(category);
            }
        }
    }
}
