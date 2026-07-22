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

namespace ElearningPlatform.Application.Features.Reviews.Queries.GetReviewStatistics
{
    public class GetReviewStatisticsQueryHandler
     : IRequestHandler<GetReviewStatisticsQuery,
         Result<ReviewStatisticsDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetReviewStatisticsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<ReviewStatisticsDto>> Handle(
            GetReviewStatisticsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<ReviewStatisticsDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required.");
            }

            if (!currentUserService.IsInRole("Admin"))
            {
                return Result<ReviewStatisticsDto>.Failure(
                    ResultStatus.Forbidden,
                    "Only administrators can access review statistics.");
            }

            var query = unitOfWork.Reviews.Query()
                .Where(x => !x.IsDeleted);

            var totalReviews = await query.CountAsync(cancellationToken);

            var approvedQuery = query.Where(x => x.Status == ReviewStatus.Approved);

            var dto = new ReviewStatisticsDto
            {
                TotalReviews = totalReviews,

                PendingReviews = await query.CountAsync(
                    x => x.Status == ReviewStatus.Pending,
                    cancellationToken),

                ApprovedReviews = await approvedQuery.CountAsync(
                    cancellationToken),

                RejectedReviews = await query.CountAsync(
                    x => x.Status == ReviewStatus.Rejected,
                    cancellationToken),

                AverageRating = await approvedQuery.AnyAsync(cancellationToken)
                    ? Math.Round(await approvedQuery.AverageAsync(
                        x => x.Rating,
                        cancellationToken), 1)
                    : 0
            };

            return Result<ReviewStatisticsDto>.Success(dto);
        }
    }
   
}
