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

namespace ElearningPlatform.Application.Features.Categories.Queries.GetCategoryByName
{
    public class GetCategoryByNameQueryHandler : IRequestHandler<GetCategoryByNameQuery, Result<CategoryDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetCategoryByNameQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<CategoryDto>> Handle(GetCategoryByNameQuery request, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.Categories.Query()
      .AsNoTracking()
      .Where(c => !c.IsDeleted && c.Name.Contains(request.Name))
      .Select(c => new CategoryDto
      {
          Id = c.Id,
          Name = c.Name,
          CoursesCount = c.Courses.Count(course => !course.IsDeleted)
      })
      .FirstOrDefaultAsync(cancellationToken);

            if (category == null)
                return Result<CategoryDto>.Failure(ResultStatus.NotFound, "Category not found");

            return Result<CategoryDto>.Success(category);

        }
    }
}
