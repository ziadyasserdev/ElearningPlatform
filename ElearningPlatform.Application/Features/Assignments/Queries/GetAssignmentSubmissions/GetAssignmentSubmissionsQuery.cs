using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Assignments.Dtos;
using ElearningPlatform.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Queries.GetAssignmentSubmissions
{
    public record GetAssignmentSubmissionsQuery(
     int AssignmentId,
     int PageNumber = 1,
     int PageSize = 10,
     string? Search = null,
     SubmissionStatus? Status = null)
     : IRequest<Result<PaginatedResult<AssignmentSubmissionDto>>>;
}
