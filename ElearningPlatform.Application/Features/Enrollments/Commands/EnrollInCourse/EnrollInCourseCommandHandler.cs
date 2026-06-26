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

namespace ElearningPlatform.Application.Features.Enrollments.Commands.EnrollInCourse
{
    public class EnrollInCourseCommandHandler
      : IRequestHandler<EnrollInCourseCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public EnrollInCourseCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            EnrollInCourseCommand request,
            CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "Authentication required");
            var userId = currentUserService.UserId;

           
            var course = await unitOfWork.Courses.Query()
                .FirstOrDefaultAsync(x =>
                    x.Id == request.CourseId &&
                    !x.IsDeleted &&
                    x.IsActive &&
                    x.Status == CourseStatus.Published,
                    cancellationToken);

            if (course is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Course not found");

           
            var alreadyEnrolled = await unitOfWork.Enrollments.Query()
                .AnyAsync(x =>
                    x.CourseId == request.CourseId &&
                    x.StudentId == userId,
                    cancellationToken);

            if (alreadyEnrolled)
                return Result<string>.Failure(ResultStatus.Conflict, "Already enrolled in this course");

        
            var enrollment = new Enrollment
            {
                CourseId = request.CourseId,
                CreatedAt = DateTime.Now,
                CreatedBy = currentUserService.UserName,
                StudentId = userId,
                EnrolledAt = DateTime.Now,
                Status = EnrollmentStatus.Active,
                ProgressPercentage = 0
            };

            await unitOfWork.Enrollments.AddAsync(enrollment);
            course.TotalStudents++;

            await unitOfWork.SaveAsync();

            return Result<string>.Success( "Enrolled successfully");
        }
    }
}
