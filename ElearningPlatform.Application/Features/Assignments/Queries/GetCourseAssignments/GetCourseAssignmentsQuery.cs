using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Assignments.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Queries.GetCourseAssignments
{

    public record GetCourseAssignmentsQuery(
       int CourseId,
       string? Search,
       bool? IsPublished,
       int PageNumber = 1,
       int PageSize = 10)
       : IRequest<Result<PaginatedResult<AssignmentListDto>>>;
}
