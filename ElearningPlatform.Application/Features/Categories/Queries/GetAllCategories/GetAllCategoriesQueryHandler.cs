using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Categories.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Queries.GetAllCategories
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, Result<List<object>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<List<object>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Categories.Query();
            if(currentUserService.IsInRole("Admin"))
            {
                var categories = await query.Select(c => new CategoryForAdminDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Slug = c.Slug,
                    Description = c.Description,
                    IsDeleted = c.IsDeleted,
                    IsActive = c.IsActive,
                    CoursesCount = c.Courses.Count(),
                    IconUrl = c.IconUrl

                }).ToListAsync(cancellationToken);

               return Result<List<object>>.Success(categories.Cast<object>().ToList());
            }
            else
            {
                var categories = await query.Where(c => c.IsActive && !c.IsDeleted)
                    .Select(c => new CategoryForUserDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Slug = c.Slug,
                        Description = c.Description,
                        IconUrl = c.IconUrl
                    }).ToListAsync(cancellationToken);
                   
                return Result<List<object>>.Success(categories.Cast<object>().ToList());

            }
        }
    }
}
