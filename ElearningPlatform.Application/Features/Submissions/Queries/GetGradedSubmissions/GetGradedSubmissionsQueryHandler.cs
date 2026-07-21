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

namespace ElearningPlatform.Application.Features.Submissions.Queries.GetGradedSubmissions
{
    public class GetGradedSubmissionsQueryHandler
         : IRequestHandler<
             GetGradedSubmissionsQuery,
             Result<PaginatedResult<GradedSubmissionDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetGradedSubmissionsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<PaginatedResult<GradedSubmissionDto>>> Handle(
            GetGradedSubmissionsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<PaginatedResult<GradedSubmissionDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

            var userId = currentUserService.UserId;

          
            var assignmentExists = await unitOfWork.Assignments
                .Query()
                .AnyAsync(a =>
                    a.Id == request.AssignmentId &&
                    !a.IsDeleted &&
                    a.Course.Instructor.UserId == userId,
                    cancellationToken);

            if (!assignmentExists)
            {
                return Result<PaginatedResult<GradedSubmissionDto>>.Failure(
                    ResultStatus.NotFound,
                    "Assignment not found.");
            }

            var query = unitOfWork.Submissions
                .Query()
                .AsNoTracking()
                .Where(s =>
                    s.AssignmentId == request.AssignmentId &&
                    !s.IsDeleted &&
                    s.Status == SubmissionStatus.Graded);

          
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = query.Where(s =>
                    s.Student.FullName.Contains(search) ||
                    s.Student.Email!.Contains(search));
            }

          
            if (request.IsLate.HasValue)
            {
                query = query.Where(s =>
                    s.IsLate == request.IsLate.Value);
            }

            
            if (request.MinScore.HasValue)
            {
                query = query.Where(s =>
                    s.Score >= request.MinScore.Value);
            }

          
            if (request.MaxScore.HasValue)
            {
                query = query.Where(s =>
                    s.Score <= request.MaxScore.Value);
            }


            query = request.SortBy switch
            {
                SubmissionSortBy.SubmittedAt =>
                    request.Descending
                        ? query.OrderByDescending(s => s.SubmittedAt)
                        : query.OrderBy(s => s.SubmittedAt),

                SubmissionSortBy.Score =>
                    request.Descending
                        ? query.OrderByDescending(s => s.Score)
                        : query.OrderBy(s => s.Score),

                SubmissionSortBy.AssignmentTitle =>
                    request.Descending
                        ? query.OrderByDescending(s => s.Assignment.Title)
                        : query.OrderBy(s => s.Assignment.Title),

                SubmissionSortBy.StudentName =>
                    request.Descending
                        ? query.OrderByDescending(s => s.Student.FullName)
                        : query.OrderBy(s => s.Student.FullName),

                SubmissionSortBy.GradedAt =>
                    request.Descending
                        ? query.OrderByDescending(s => s.GradedAt)
                        : query.OrderBy(s => s.GradedAt),

                _ =>
                    query.OrderByDescending(s => s.GradedAt)
            };

            var totalCount = await query
                .CountAsync(cancellationToken);

            var submissions = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(s => new GradedSubmissionDto
                {
                    Id = s.Id,

                    AssignmentId = s.AssignmentId,

                    AssignmentTitle = s.Assignment.Title,

                    StudentId = s.StudentId,

                    StudentName = s.Student.FullName,

                    StudentEmail = s.Student.Email,

                    FileName = s.FileName,

                    FileSize = s.FileSize,

                    Comment = s.Comment,

                    Score = s.Score!.Value,

                    MaxScore = s.Assignment.MaxScore,

                    Feedback = s.Feedback,

                    IsLate = s.IsLate,

                    SubmittedAt = s.SubmittedAt,

                    GradedAt = s.GradedAt!.Value
                })
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<GradedSubmissionDto>(
                submissions,
                totalCount,
                request.PageNumber,
                request.PageSize);

            return Result<PaginatedResult<GradedSubmissionDto>>
                .Success(result);
        }
    }
}
