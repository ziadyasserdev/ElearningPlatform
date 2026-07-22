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

namespace ElearningPlatform.Application.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommandHandler
       : IRequestHandler<UpdateReviewCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public UpdateReviewCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            UpdateReviewCommand request,
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
                    "Only students can update reviews.");
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
                    "You can update only your review.");
            }

            review.Rating = request.Rating;
            review.Comment = request.Comment.Trim();

        
            review.Status = ReviewStatus.Pending;

            review.RejectionReason = null;
            review.ReviewedAt = null;
            review.ReviewedBy = null;

            review.EditedAt = DateTime.Now;

            review.UpdatedAt = DateTime.Now;
            review.UpdatedBy = currentUserService.UserName;

         

            await unitOfWork.SaveAsync();

            return Result<string>.Success(
                "Review updated successfully and sent for approval.");
        }
    }
}
