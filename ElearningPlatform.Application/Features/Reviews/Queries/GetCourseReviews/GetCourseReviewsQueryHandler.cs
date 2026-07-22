using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Reviews.Dtos;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Reviews.Queries.GetCourseReviews
{
    public class GetCourseReviewsQueryHandler
          : IRequestHandler<GetCourseReviewsQuery,
          Result<PaginatedResult<CourseReviewDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetCourseReviewsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedResult<CourseReviewDto>>> Handle(
            GetCourseReviewsQuery request,
            CancellationToken cancellationToken)
        {
            var courseExists = await unitOfWork.Courses.Query()
                .AnyAsync(x =>
                    x.Id == request.CourseId &&
                    !x.IsDeleted,
                    cancellationToken);

            if (!courseExists)
            {
                return Result<PaginatedResult<CourseReviewDto>>
                    .Failure(ResultStatus.NotFound,
                    "Course not found.");
            }

            var query = unitOfWork.Reviews.Query()
                .AsNoTracking()
                .Where(x =>
                    x.CourseId == request.CourseId &&
                    x.Status == ReviewStatus.Approved &&
                    !x.IsDeleted)
                .Include(x => x.Student)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x =>
                    x.Student.FullName.Contains(request.Search));
            }

            if (request.Rating.HasValue)
            {
                query = query.Where(x =>
                    x.Rating == request.Rating.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var reviews = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new CourseReviewDto
                {
                    Id = x.Id,
                    StudentName = x.Student.FullName,
                    Rating = x.Rating,
                    Comment = x.Comment,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return Result<PaginatedResult<CourseReviewDto>>
                .Success(new PaginatedResult<CourseReviewDto>
               (reviews,request.PageNumber,request.PageSize,totalCount));
        }
    }
}
