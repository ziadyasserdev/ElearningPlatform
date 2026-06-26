using ElearningPlatform.Application.Common.Results;
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

namespace ElearningPlatform.Application.Features.Categories.Queries.GetCategoryStats
{
    public class GetCategoryStatsQueryHandler : IRequestHandler<GetCategoryStatsQuery, Result<CategoryStatsDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetCategoryStatsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<CategoryStatsDto>> Handle(GetCategoryStatsQuery request, CancellationToken cancellationToken)
        {
            var stats = await unitOfWork.Courses.Query()
        .Where(c => c.CategoryId == request.CategoryId
                    && !c.IsDeleted
                    && c.Status == CourseStatus.Published)
        .GroupBy(c => c.CategoryId)
        .Select(g => new CategoryStatsDto
        {
            TotalCourses = g.Count(),

            TotalStudents = g.SelectMany(c => c.Enrollments)
                             .Select(e => e.StudentId)
                             .Distinct()
                             .Count(),

            AverageRating = g.Sum(c => c.AverageRating * c.TotalReviews) /
                           (g.Sum(c => c.TotalReviews) == 0
                                ? 1
                                : g.Sum(c => c.TotalReviews))
        })
        .FirstOrDefaultAsync(cancellationToken);

            if (stats == null)
            {
                stats = new CategoryStatsDto
                {
                    TotalCourses = 0,
                    TotalStudents = 0,
                    AverageRating = 0
                };
            }

           return Result<CategoryStatsDto>.Success(stats);
        }
    }
}
