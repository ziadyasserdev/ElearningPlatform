using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Enrollments.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Queries.GetEnrollmentDetails
{
    public class GetEnrollmentDetailsQueryHandler : IRequestHandler<GetEnrollmentDetailsQuery, Result<EnrollmentDetailsDto>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetEnrollmentDetailsQueryHandler(IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this._httpContextAccessor = httpContextAccessor;
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<EnrollmentDetailsDto>> Handle(GetEnrollmentDetailsQuery request, CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<EnrollmentDetailsDto>.Failure(ResultStatus.Unauthorized, "Authentication required");
            var userId = currentUserService.UserId;
            var enrollment = await unitOfWork.Enrollments.Query()
               .Where(e =>
                   e.CourseId == request.CourseId &&
                   e.StudentId == userId)
               .Select(e => new
               {
                   Enrollment = e,
                   Course = e.Course
               })
               .FirstOrDefaultAsync(cancellationToken);

            if (enrollment is null)
                return Result<EnrollmentDetailsDto>.Failure(
                    ResultStatus.NotFound,
                    "Enrollment not found");

            var totalVideos = await unitOfWork.Videos.Query()
                .CountAsync(v =>
                    v.Lesson.Section.CourseId == request.CourseId &&
                    !v.IsDeleted,
                    cancellationToken);

            var completedVideos = await unitOfWork.VideoProgresses.Query()
                .CountAsync(v =>
                    v.UserId == userId &&
                    v.IsCompleted &&
                    v.Video.Lesson.Section.CourseId == request.CourseId,
                    cancellationToken);
            
             
             
            
            var dto = new EnrollmentDetailsDto
            {
                CourseId = enrollment.Course.Id,
                CourseTitle = enrollment.Course.Title,
                InstructorName = enrollment.Course.Instructor.User.FullName,
                CategoryName = enrollment.Course.Category.Name,
                ThumbnailUrl = $"{_httpContextAccessor.HttpContext!.Request.Scheme}" +
                $"://{_httpContextAccessor.HttpContext!.Request.Host}" +
                $"/{enrollment.Course.ThumbnailUrl!}",
                Price = enrollment.Course.Price,
                DiscountPrice = enrollment.Course.DiscountPrice,

                ProgressPercentage =
                    enrollment.Enrollment.ProgressPercentage,

                Status =
                    enrollment.Enrollment.Status.ToString(),

                EnrolledAt =
                    enrollment.Enrollment.EnrolledAt,

                CompletedAt =
                    enrollment.Enrollment.CompletedAt,

                TotalLessons =
                    enrollment.Course.TotalLessons,

                TotalVideos =
                    totalVideos,

                CompletedVideos =
                    completedVideos
            };

            return Result<EnrollmentDetailsDto>
                .Success(dto);
        }
    }
}
