using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Reviews.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Reviews.Queries.GetDeletedReviews
{
    public class GetDeletedReviewsQueryHandler
      : IRequestHandler<GetDeletedReviewsQuery,
          Result<PaginatedResult<AdminReviewDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetDeletedReviewsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<PaginatedResult<AdminReviewDto>>> Handle(
            GetDeletedReviewsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<PaginatedResult<AdminReviewDto>>
                    .Failure(ResultStatus.Unauthorized,
                    "Authentication required.");
            }

            if (!currentUserService.IsInRole("Admin"))
            {
                return Result<PaginatedResult<AdminReviewDto>>
                    .Failure(ResultStatus.Forbidden,
                    "Only administrators can access deleted reviews.");
            }

            IQueryable<Domain.Models.Review> query = unitOfWork.Reviews.Query()
                .IgnoreQueryFilters()
                .Where(x => x.IsDeleted)
                .Include(x => x.Student)
                .Include(x => x.Course);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x =>
                    x.Student.FullName.Contains(request.Search) ||
                    x.Student.Email.Contains(request.Search) ||
                    x.Course.Title.Contains(request.Search));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var reviews = await query
                .OrderByDescending(x => x.DeletedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new AdminReviewDto
                {
                    Id = x.Id,

                    StudentId = x.StudentId,
                    StudentName = x.Student.FullName,
                    StudentEmail = x.Student.Email!,

                    CourseId = x.CourseId,
                    CourseTitle = x.Course.Title,

                    Rating = x.Rating,
                    Comment = x.Comment,

                    Status = x.Status.ToString(),

                    ReviewedBy = x.ReviewedBy,
                    ReviewedAt = x.ReviewedAt,

                    RejectionReason = x.RejectionReason,

                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return Result<PaginatedResult<AdminReviewDto>>
                .Success(new PaginatedResult<AdminReviewDto>
             (reviews,request.PageNumber,request.PageSize,totalCount));
        }
    }
}
