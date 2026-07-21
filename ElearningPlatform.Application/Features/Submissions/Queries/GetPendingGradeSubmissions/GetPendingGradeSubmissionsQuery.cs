using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Submissions.Dtos;
using ElearningPlatform.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Queries.GetPendingGradeSubmissions
{
    public record GetPendingGradeSubmissionsQuery(
       string? Search = null,
       bool? IsLate = null,
       SubmissionSortBy SortBy = SubmissionSortBy.SubmittedAt,
       bool Descending = true,
       int PageNumber = 1,
       int PageSize = 10)
       : IRequest<Result<PaginatedResult<PendingGradeSubmissionDto>>>;
}
