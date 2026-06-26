using ElearningPlatform.Application.Common.PaginatedResults;
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

namespace ElearningPlatform.Application.Features.Categories.Queries.GetAllCategoriesWithPagination
{
    public class GetAllCategoriesWithPaginationQueryHandler : IRequestHandler<GetAllCategoriesWithPaginationQuery, Result<PaginatedResult<object>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetAllCategoriesWithPaginationQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<PaginatedResult<object>>> Handle(GetAllCategoriesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Categories.Query();

            if (currentUserService.IsInRole("Admin"))
            {
                var totalCount = await query.CountAsync(cancellationToken);

                var categories = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(c => new CategoryForAdminDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Slug = c.Slug,
                        Description = c.Description,
                        IsDeleted = c.IsDeleted,
                        IsActive = c.IsActive,
                        CoursesCount = c.Courses.Count(),
                        IconUrl = c.IconUrl
                    })
                    .ToListAsync(cancellationToken);

                return Result<PaginatedResult<object>>.Success(
                    new PaginatedResult<object>(categories.Cast<object>().ToList(), request.PageNumber, request.PageSize, totalCount)
                );
            }
            else
            {
                var filteredQuery = query.Where(c => c.IsActive && !c.IsDeleted);
                var totalCount = await filteredQuery.CountAsync(cancellationToken);

                var categories = await filteredQuery
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(c => new CategoryForUserDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Slug = c.Slug,
                        Description = c.Description,
                        IconUrl = c.IconUrl
                    })
                    .ToListAsync(cancellationToken);

                return Result<PaginatedResult<object>>.Success(
                    new PaginatedResult<object>(categories.Cast<object>().ToList(), request.PageNumber, request.PageSize, totalCount)
                );
            }
        }
    }
}
