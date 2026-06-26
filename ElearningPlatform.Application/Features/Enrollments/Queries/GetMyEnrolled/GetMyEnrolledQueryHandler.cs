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

namespace ElearningPlatform.Application.Features.Enrollments.Queries.GetMyEnrolled
{
    public class GetMyEnrolledCoursesQueryHandler
      : IRequestHandler<GetMyEnrolledCoursesQuery,
          Result<List<MyEnrolledCourseDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyEnrolledCoursesQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<List<MyEnrolledCourseDto>>> Handle(
            GetMyEnrolledCoursesQuery request,
            CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<List<MyEnrolledCourseDto>>.Failure(ResultStatus.Unauthorized, "Authentication required");
            var userId = currentUserService.UserId;
            // e.Course.ThumbnailUrl!
            var enrollments = await unitOfWork.Enrollments.Query()
                .Where(e =>
                    e.StudentId == userId &&
                    !e.Course.IsDeleted &&
                    e.Course.IsActive)
                .OrderByDescending(e => e.EnrolledAt)
                .Select(e => new MyEnrolledCourseDto
                {
                    CourseId = e.CourseId,
                    CourseTitle = e.Course.Title,
                    InstructorName = e.Course.Instructor.User.FullName,
                    CategoryName = e.Course.Category.Name,
                    ThumbnailUrl = $"{_httpContextAccessor.HttpContext!.Request.Scheme}" +
                    $"://{_httpContextAccessor.HttpContext!.Request.Host}" +
                    $"/{e.Course.ThumbnailUrl!}",
                    ProgressPercentage = e.ProgressPercentage,
                    Status = e.Status.ToString(),
                    EnrolledAt = e.EnrolledAt,
                    Price = e.Course.Price,
                    DiscountPrice = e.Course.DiscountPrice
                })
                .ToListAsync(cancellationToken);

            return Result<List<MyEnrolledCourseDto>>
                .Success(enrollments);
        }
    }
}
