using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Enums;
using ElearningPlatform.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommandHandler
      : IRequestHandler<CreateReviewCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public CreateReviewCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(
            CreateReviewCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<int>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required.");
            }

            if (!currentUserService.IsInRole("Student"))
            {
                return Result<int>.Failure(
                    ResultStatus.Forbidden,
                    "Only students can review courses.");
            }

            var studentId = currentUserService.UserId;

            var enrolled = await unitOfWork.Enrollments.Query()
                .AnyAsync(x =>
                    x.StudentId == studentId &&
                    x.CourseId == request.CourseId,
                    cancellationToken);

            if (!enrolled)
            {
                return Result<int>.Failure(
                    ResultStatus.Forbidden,
                    "You are not enrolled in this course.");
            }

            var reviewExists = await unitOfWork.Reviews.Query()
                .AnyAsync(x =>
                    x.StudentId == studentId &&
                    x.CourseId == request.CourseId &&
                    !x.IsDeleted,
                    cancellationToken);

            if (reviewExists)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "You already reviewed this course.");
            }

            var review = new Review
            {
                StudentId = studentId!,
                CourseId = request.CourseId,
                Rating = request.Rating,
                Comment = request.Comment.Trim(),

                Status = ReviewStatus.Pending,

                CreatedAt = DateTime.Now,
                CreatedBy = currentUserService.UserName
            };

            await unitOfWork.Reviews.AddAsync(review);

            await unitOfWork.SaveAsync();

            return Result<int>.Success(review.Id);
        }
    }
}
