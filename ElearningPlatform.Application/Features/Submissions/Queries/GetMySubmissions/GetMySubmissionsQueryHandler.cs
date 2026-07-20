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

namespace ElearningPlatform.Application.Features.Submissions.Queries.GetMySubmissions
{
    public class GetMySubmissionsQueryHandler
         : IRequestHandler<GetMySubmissionsQuery, Result<PaginatedResult<MySubmissionListDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetMySubmissionsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<PaginatedResult<MySubmissionListDto>>> Handle(
            GetMySubmissionsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<PaginatedResult<MySubmissionListDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

            var studentId = currentUserService.UserId;

            var query = unitOfWork.Submissions
                .Query()
                .AsNoTracking()
                .Where(s =>
                    s.StudentId == studentId &&
                    !s.IsDeleted);

            if (request.Status.HasValue)
            {
                query = query.Where(s =>
                    s.Status == request.Status.Value);
            }

            if (request.IsLate.HasValue)
            {
                query = query.Where(s =>
                    s.IsLate == request.IsLate.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim().ToLower();

                query = query.Where(s =>
                    s.Assignment.Title.ToLower().Contains(search) ||
                    s.Assignment.Course.Title.ToLower().Contains(search));
            }

            query = request.SortBy switch
            {
                SubmissionSortBy.Score => request.Descending
                    ? query.OrderByDescending(x => x.Score)
                    : query.OrderBy(x => x.Score),

                SubmissionSortBy.AssignmentTitle => request.Descending
                    ? query.OrderByDescending(x => x.Assignment.Title)
                    : query.OrderBy(x => x.Assignment.Title),

                _ => request.Descending
                    ? query.OrderByDescending(x => x.SubmittedAt)
                    : query.OrderBy(x => x.SubmittedAt)
            };

            var totalCount = await query.CountAsync(cancellationToken);

            var submissions = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(s => new MySubmissionListDto
                {
                    Id = s.Id,
                    AssignmentId = s.AssignmentId,
                    AssignmentTitle = s.Assignment.Title,
                    CourseId = s.Assignment.CourseId,
                    CourseTitle = s.Assignment.Course.Title,
                    FileName = s.FileName,
                    Score = s.Score,
                    IsLate = s.IsLate,
                    Status = s.Status,
                    SubmittedAt = s.SubmittedAt,
                    GradedAt = s.GradedAt
                })
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<MySubmissionListDto>(
                submissions,
                totalCount,
                request.PageNumber,
                request.PageSize);

            return Result<PaginatedResult<MySubmissionListDto>>
                .Success(result);
        }
    }
}
