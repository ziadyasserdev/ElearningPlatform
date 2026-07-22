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

namespace ElearningPlatform.Application.Features.Reviews.Queries.GetMyReviews
{
    public class GetMyReviewsQueryHandler
       : IRequestHandler<GetMyReviewsQuery,
           Result<PaginatedResult<MyReviewDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetMyReviewsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<PaginatedResult<MyReviewDto>>> Handle(
            GetMyReviewsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<PaginatedResult<MyReviewDto>>
                    .Failure(ResultStatus.Unauthorized,
                    "Authentication required.");
            }

            var query = unitOfWork.Reviews.Query()
                .AsNoTracking()
                   .Include(x => x.Course)
                .Where(x =>
                    x.StudentId == currentUserService.UserId &&
                    !x.IsDeleted)
            ;

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x =>
                    x.Course.Title.Contains(request.Search));
            }

            if (request.Status.HasValue)
            {
                query = query.Where(x =>
                    x.Status == request.Status.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var reviews = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new MyReviewDto
                {
                    Id = x.Id,

                    CourseId = x.CourseId,

                    CourseTitle = x.Course.Title,

                    Rating = x.Rating,

                    Comment = x.Comment,

                    Status = x.Status.ToString(),

                    RejectionReason = x.RejectionReason,

                    CreatedAt = x.CreatedAt,

                    EditedAt = x.EditedAt
                })
                .ToListAsync(cancellationToken);

            return Result<PaginatedResult<MyReviewDto>>.Success(new PaginatedResult<MyReviewDto>(reviews,
                request.PageNumber,request.PageSize,totalCount));
              
        }
    }
}
