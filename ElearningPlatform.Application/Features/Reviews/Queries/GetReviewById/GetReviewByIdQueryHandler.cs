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

namespace ElearningPlatform.Application.Features.Reviews.Queries.GetReviewById
{
    public class GetReviewByIdQueryHandler
       : IRequestHandler<GetReviewByIdQuery, Result<AdminReviewDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetReviewByIdQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<AdminReviewDto>> Handle(
            GetReviewByIdQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<AdminReviewDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required.");
            }

            if (!currentUserService.IsInRole("Admin"))
            {
                return Result<AdminReviewDto>.Failure(
                    ResultStatus.Forbidden,
                    "Only administrators can access reviews.");
            }

            var review = await unitOfWork.Reviews.Query()
                .AsNoTracking()
                .Include(x => x.Student)
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x =>
                    x.Id == request.Id &&
                    !x.IsDeleted,
                    cancellationToken);

            if (review == null)
            {
                return Result<AdminReviewDto>.Failure(
                    ResultStatus.NotFound,
                    "Review not found.");
            }

            var dto = new AdminReviewDto
            {
                Id = review.Id,

                CourseId = review.CourseId,
                CourseTitle = review.Course.Title,

                StudentId = review.StudentId,
                StudentName = review.Student.FullName,
                StudentEmail = review.Student.Email!,

                Rating = review.Rating,
                Comment = review.Comment,

                Status = review.Status.ToString(),

                ReviewedBy = review.ReviewedBy,
                ReviewedAt = review.ReviewedAt,

                RejectionReason = review.RejectionReason,

                CreatedAt = review.CreatedAt
            };

            return Result<AdminReviewDto>.Success(dto);
        }
    }
}
