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

namespace ElearningPlatform.Application.Features.Reviews.Commands.RejectReview
{
    public class RejectReviewCommandHandler
      : IRequestHandler<RejectReviewCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public RejectReviewCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            RejectReviewCommand request,
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
                    "Only administrators can reject reviews.");
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

            if (review.Status == ReviewStatus.Approved)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Approved reviews cannot be rejected.");
            }

            review.Status = ReviewStatus.Rejected;
            review.RejectionReason = request.RejectionReason.Trim();

            review.ReviewedBy = currentUserService.UserName;
            review.ReviewedAt = DateTime.Now;

            review.UpdatedAt = DateTime.Now;
            review.UpdatedBy = currentUserService.UserName;

            unitOfWork.Reviews.Update(review);

        

            return Result<string>.Success(
                "Review rejected successfully.");
        }
    }
}
