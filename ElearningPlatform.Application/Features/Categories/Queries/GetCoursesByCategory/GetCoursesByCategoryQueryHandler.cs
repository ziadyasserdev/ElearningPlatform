using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Categories.Dtos;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Queries.GetCoursesByCategory
{
    public class GetCoursesByCategoryQueryHandler : IRequestHandler<GetCoursesByCategoryQuery, Result<PaginatedResult<object>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetCoursesByCategoryQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<PaginatedResult<object>>> Handle(GetCoursesByCategoryQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Courses.Query()
                .Where(x => x.CategoryId == request.CategoryId);
            if (currentUserService.IsInRole("Admin"))
            {
                var courses = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new CourseForAdminDto
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Description = x.Description,
                        IsDeleted = x.IsDeleted,
                        IsActive = x.IsActive,
                        Slug = x.Slug,
                        CreatedAt = x.CreatedAt,
                        StudentsCount = x.Enrollments.Count()
                    })
                    .ToListAsync(cancellationToken);
                return Result<PaginatedResult<object>>.Success(new PaginatedResult<object>(courses, request.PageNumber, request.PageSize, await query.CountAsync(cancellationToken)));
            }
            else
            {
                var courses = await query
                    .Where(x => x.IsActive && !x.IsDeleted)
                   .Skip((request.PageNumber - 1) * request.PageSize)
                   .Take(request.PageSize)
                   .Select(x => new CourseForAdminDto
                   {
                       Id = x.Id,
                       Title = x.Title,
                       Description = x.Description,
                       IsDeleted = x.IsDeleted,
                       IsActive = x.IsActive,
                       Slug = x.Slug,
                       CreatedAt = x.CreatedAt,
                       StudentsCount = x.Enrollments.Count()
                   })
                   .ToListAsync(cancellationToken);

                return Result<PaginatedResult<object>>.Success(new PaginatedResult<object>(courses, request.PageNumber, request.PageSize, await query.CountAsync(cancellationToken)));

            }

        }
    }
}
