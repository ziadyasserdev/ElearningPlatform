using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Reviews.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Reviews.Queries.GetCourseReviews
{
    public class GetCourseReviewsQuery
      : IRequest<Result<PaginatedResult<CourseReviewDto>>>
    {
        public int CourseId { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public int? Rating { get; set; }

        public string? Search { get; set; }
    }
}
