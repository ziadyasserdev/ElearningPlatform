using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Reviews.Dtos;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Reviews.Queries.GetPendingReviews
{
    public class GetPendingReviewsQueryHandler
      : IRequestHandler<GetPendingReviewsQuery,
          Result<PaginatedResult<PendingReviewDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetPendingReviewsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<PaginatedResult<PendingReviewDto>>> Handle(
            GetPendingReviewsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<PaginatedResult<PendingReviewDto>>
                    .Failure(ResultStatus.Unauthorized,
                    "Authentication required.");
            }

            if (!currentUserService.IsInRole("Admin"))
            {
                return Result<PaginatedResult<PendingReviewDto>>
                    .Failure(ResultStatus.Forbidden,
                    "Only administrators can access pending reviews.");
            }

            var query = unitOfWork.Reviews.Query()
                .AsNoTracking()
                .Include(x => x.Student)
                .Include(x => x.Course)
                .Where(x =>
                    x.Status == ReviewStatus.Pending &&
                    !x.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x =>
                    x.Student.FullName.Contains(request.Search) ||
                    x.Student.Email.Contains(request.Search) ||
                    x.Course.Title.Contains(request.Search));
            }

            if (request.Rating.HasValue)
            {
                query = query.Where(x =>
                    x.Rating == request.Rating.Value);
            }

            if (request.From.HasValue)
            {
                query = query.Where(x =>
                    x.CreatedAt >= request.From.Value);
            }

            if (request.To.HasValue)
            {
                query = query.Where(x =>
                    x.CreatedAt <= request.To.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var reviews = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new PendingReviewDto
                {
                    Id = x.Id,
                    CourseId = x.CourseId,
                    CourseTitle = x.Course.Title,
                    StudentId = x.StudentId,
                    StudentName = x.Student.FullName,
                    StudentEmail = x.Student.Email!,
                    Rating = x.Rating,
                    Comment = x.Comment,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return Result<PaginatedResult<PendingReviewDto>>
                .Success(new PaginatedResult<PendingReviewDto>
               (reviews,request.PageNumber,request.PageSize,totalCount));
        }
    }
}
