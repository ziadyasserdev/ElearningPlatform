using ElearningPlatform.Application.Common.Results;
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

namespace ElearningPlatform.Application.Features.Reviews.Queries.GetCourseReviewSummary
{
    public class GetCourseReviewSummaryQueryHandler
      : IRequestHandler<GetCourseReviewSummaryQuery,
          Result<CourseReviewSummaryDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetCourseReviewSummaryQueryHandler(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<CourseReviewSummaryDto>> Handle(
            GetCourseReviewSummaryQuery request,
            CancellationToken cancellationToken)
        {
            var courseExists = await unitOfWork.Courses.Query()
                .AnyAsync(x =>
                    x.Id == request.CourseId &&
                    !x.IsDeleted,
                    cancellationToken);

            if (!courseExists)
            {
                return Result<CourseReviewSummaryDto>.Failure(
                    ResultStatus.NotFound,
                    "Course not found.");
            }

            var reviews = unitOfWork.Reviews.Query()
                .Where(x =>
                    x.CourseId == request.CourseId &&
                    x.Status == ReviewStatus.Approved &&
                    !x.IsDeleted);

            var totalReviews = await reviews.CountAsync(cancellationToken);

            var averageRating = totalReviews == 0
                ? 0
                : await reviews.AverageAsync(x => x.Rating, cancellationToken);

            var dto = new CourseReviewSummaryDto
            {
                AverageRating = Math.Round(averageRating, 1),

                TotalReviews = totalReviews,

                FiveStars = await reviews.CountAsync(x => x.Rating == 5, cancellationToken),

                FourStars = await reviews.CountAsync(x => x.Rating == 4, cancellationToken),

                ThreeStars = await reviews.CountAsync(x => x.Rating == 3, cancellationToken),

                TwoStars = await reviews.CountAsync(x => x.Rating == 2, cancellationToken),

                OneStar = await reviews.CountAsync(x => x.Rating == 1, cancellationToken)
            };

            return Result<CourseReviewSummaryDto>.Success(dto);
        }
    }
}
