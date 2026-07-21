using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Submissions.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Queries.GetUngradedSubmissions
{
    public record GetUngradedSubmissionsQuery(
      int AssignmentId,
      string? Search = null,
      bool? IsLate = null,
      int PageNumber = 1,
      int PageSize = 10)
      : IRequest<Result<PaginatedResult<UngradedSubmissionDto>>>;
}
