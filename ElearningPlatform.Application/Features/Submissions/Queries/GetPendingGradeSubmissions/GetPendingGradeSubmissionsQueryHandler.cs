using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Submissions.Dtos;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Queries.GetPendingGradeSubmissions
{
    public class GetPendingGradeSubmissionsQueryHandler
          : IRequestHandler<
              GetPendingGradeSubmissionsQuery,
              Result<PaginatedResult<PendingGradeSubmissionDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetPendingGradeSubmissionsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<PaginatedResult<PendingGradeSubmissionDto>>> Handle(
            GetPendingGradeSubmissionsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<PaginatedResult<PendingGradeSubmissionDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

            var userId = currentUserService.UserId;

            var query = unitOfWork.Submissions
                .Query()
                .AsNoTracking()
                .Where(s =>
                    !s.IsDeleted &&
                    s.Status != SubmissionStatus.Graded &&
                    s.Assignment.Course.Instructor.UserId == userId);

         
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = query.Where(s =>
                    s.Student.FullName.Contains(search) ||
                    s.Student.Email!.Contains(search) ||
                    s.Assignment.Title.Contains(search) ||
                    s.Assignment.Course.Title.Contains(search));
            }

            if (request.IsLate.HasValue)
            {
                query = query.Where(s =>
                    s.IsLate == request.IsLate.Value);
            }

          
            query = request.SortBy switch
            {
                SubmissionSortBy.SubmittedAt =>
                    request.Descending
                        ? query.OrderByDescending(s => s.SubmittedAt)
                        : query.OrderBy(s => s.SubmittedAt),

                SubmissionSortBy.StudentName =>
                    request.Descending
                        ? query.OrderByDescending(s => s.Student.FullName)
                        : query.OrderBy(s => s.Student.FullName),

                SubmissionSortBy.AssignmentTitle =>
                    request.Descending
                        ? query.OrderByDescending(s => s.Assignment.Title)
                        : query.OrderBy(s => s.Assignment.Title),

                _ =>
                    query.OrderByDescending(s => s.SubmittedAt)
            };

            var totalCount = await query
                .CountAsync(cancellationToken);

            var submissions = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(s => new PendingGradeSubmissionDto
                {
                    Id = s.Id,

                    AssignmentId = s.AssignmentId,

                    AssignmentTitle = s.Assignment.Title,

                    CourseId = s.Assignment.CourseId,

                    CourseTitle = s.Assignment.Course.Title,

                    StudentId = s.StudentId,

                    StudentName = s.Student.FullName,

                    StudentEmail = s.Student.Email,

                    FileName = s.FileName,

                    Comment = s.Comment,

                    IsLate = s.IsLate,

                    SubmittedAt = s.SubmittedAt
                })
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<PendingGradeSubmissionDto>(
                submissions,
                totalCount,
                request.PageNumber,
                request.PageSize);

            return Result<PaginatedResult<PendingGradeSubmissionDto>>
                .Success(result);
        }
    }
}
