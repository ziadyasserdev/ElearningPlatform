using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Assignments.Dtos;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Queries.GetCourseAssignments
{
    public class GetCourseAssignmentsQueryHandler
    : IRequestHandler<GetCourseAssignmentsQuery,
        Result<PaginatedResult<AssignmentListDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetCourseAssignmentsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<PaginatedResult<AssignmentListDto>>> Handle(
            GetCourseAssignmentsQuery request,
            CancellationToken cancellationToken)
        {
            var courseExists = await unitOfWork.Courses
                .Query()
                .AnyAsync(c =>
                    c.Id == request.CourseId &&
                    !c.IsDeleted,
                    cancellationToken);

            if (!courseExists)
                return Result<PaginatedResult<AssignmentListDto>>
                    .Failure(ResultStatus.NotFound, "Course not found.");

            var query = unitOfWork.Assignments
                .Query()
                .Where(a =>
                    a.CourseId == request.CourseId &&
                    !a.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(a =>
                    a.Title.Contains(request.Search));
            }

            if (request.IsPublished.HasValue)
            {
                query = query.Where(a =>
                    a.IsPublished == request.IsPublished.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(a => a.DueDate)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(a => new AssignmentListDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    MaxScore = a.MaxScore,
                    OpenAt = a.OpenAt,
                    DueDate = a.DueDate,
                    IsPublished = a.IsPublished,
                    SubmissionCount = a.Submissions.Count
                })
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<AssignmentListDto>(
                items,
                request.PageNumber,
                request.PageSize,
                totalCount);

            return Result<PaginatedResult<AssignmentListDto>>
                .Success(result);
        }
    }
}
