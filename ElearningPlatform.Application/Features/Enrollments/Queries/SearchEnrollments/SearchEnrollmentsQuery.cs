using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Enrollments.Dtos;
using ElearningPlatform.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Queries.SearchEnrollments
{

    public class SearchEnrollmentsQuery
        : IRequest<Result<PaginatedResult<AdminEnrollmentDto>>>
    {
        public string? Search { get; set; }

        public EnrollmentStatus? Status { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
