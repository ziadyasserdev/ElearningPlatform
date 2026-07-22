using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Reviews.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Reviews.Queries.GetPendingReviews
{
    public class GetPendingReviewsQuery
       : IRequest<Result<PaginatedResult<PendingReviewDto>>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }

        public int? Rating { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }
    }
}