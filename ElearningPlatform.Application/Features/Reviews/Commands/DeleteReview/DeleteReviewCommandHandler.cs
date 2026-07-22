using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Reviews.Commands.DeleteReview
{
    public class DeleteReviewCommandHandler
      : IRequestHandler<DeleteReviewCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public DeleteReviewCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            DeleteReviewCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required.");
            }

            if (!currentUserService.IsInRole("Student"))
            {
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "Only students can delete reviews.");
            }

            var review = await unitOfWork.Reviews.Query()
                .FirstOrDefaultAsync(x =>
                    x.Id == request.Id &&
                    !x.IsDeleted,
                    cancellationToken);

            if (review == null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Review not found.");
            }

            if (review.StudentId != currentUserService.UserId)
            {
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "You can delete only your review.");
            }

            review.IsDeleted = true;
            review.DeletedAt = DateTime.Now;
            review.DeletedBy = currentUserService.UserName;

            unitOfWork.Reviews.Update(review);

            await unitOfWork.SaveAsync();

            return Result<string>.Success(
                "Review deleted successfully.");
        }
    }
}
