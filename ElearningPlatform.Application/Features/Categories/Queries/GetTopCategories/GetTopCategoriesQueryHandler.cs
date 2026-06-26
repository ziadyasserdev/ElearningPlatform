using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Categories.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Queries.GetTopCategories
{
    public class GetTopCategoriesQueryHandler : IRequestHandler<GetTopCategoriesQuery, Result<List<TopCategoryDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetTopCategoriesQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<TopCategoryDto>>> Handle(GetTopCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await unitOfWork.Categories.Query()
                 .AsNoTracking()
                 .Where(c => !c.IsDeleted && c.IsActive)
                 .Select(c => new TopCategoryDto
                 {
                     Id = c.Id,
                     Name = c.Name,
                     IconUrl = c.IconUrl,
                     CourseCount = c.Courses.Count(x => !x.IsDeleted && x.IsActive)
                 })
                 .OrderByDescending(c => c.CourseCount)
                 .Take(request.Limit)
                 .ToListAsync(cancellationToken);

            return Result<List<TopCategoryDto>>.Success(categories);
        }
    }
}
