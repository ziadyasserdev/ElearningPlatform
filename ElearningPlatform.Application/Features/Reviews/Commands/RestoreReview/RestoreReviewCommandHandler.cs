using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Reviews.Commands.RestoreReview
{

    public class RestoreReviewCommandHandler
        : IRequestHandler<RestoreReviewCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public RestoreReviewCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            RestoreReviewCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required.");
            }

            if (!currentUserService.IsInRole("Admin"))
            {
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "Only administrators can restore reviews.");
            }

            var review = await unitOfWork.Reviews.Query()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(x =>
                    x.Id == request.Id &&
                    x.IsDeleted,
                    cancellationToken);

            if (review == null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Deleted review not found.");
            }

            review.IsDeleted = false;
            review.DeletedAt = null;
            review.DeletedBy = null;

            unitOfWork.Reviews.Update(review);

            await unitOfWork.SaveAsync();

            if (review.Status == ReviewStatus.Approved)
            {
                var course = await unitOfWork.Courses.Query()
                    .FirstOrDefaultAsync(x =>
                        x.Id == review.CourseId,
                        cancellationToken);

                if (course != null)
                {
                    var approvedReviews = unitOfWork.Reviews.Query()
                        .Where(x =>
                            x.CourseId == review.CourseId &&
                            x.Status == ReviewStatus.Approved &&
                            !x.IsDeleted);

                    course.TotalReviews = await approvedReviews.CountAsync(cancellationToken);

                    course.AverageRating = course.TotalReviews == 0
                        ? 0
                        : (decimal)await approvedReviews
                            .AverageAsync(x => x.Rating, cancellationToken);

                    unitOfWork.Courses.Update(course);

                    await unitOfWork.SaveAsync();
                }
            }

            return Result<string>.Success(
                "Review restored successfully.");
        }
    }
}
