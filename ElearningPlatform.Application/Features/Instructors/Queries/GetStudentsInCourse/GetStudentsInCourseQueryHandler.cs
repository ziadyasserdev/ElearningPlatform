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

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetStudentsInCourse
{
    public class GetStudentsInCourseQueryHandler
        : IRequestHandler<GetStudentsInCourseQuery, Result<List<CourseStudentDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetStudentsInCourseQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<CourseStudentDto>>> Handle(
            GetStudentsInCourseQuery request,
            CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<List<CourseStudentDto>>.Failure(ResultStatus.Unauthorized, "Authentication required");
            var instructorId = currentUserService.UserId;

            var course = await unitOfWork.Courses.Query()
                .FirstOrDefaultAsync(c =>
                    c.Id == request.CourseId &&
                    c.Instructor.UserId == instructorId,
                    cancellationToken);

            if (course is null)
                return Result<List<CourseStudentDto>>.Failure(
                    ResultStatus.NotFound,
                    "Course not found or not yours");

            var students = await unitOfWork.Enrollments.Query()
                .Where(e =>
                    e.CourseId == request.CourseId &&
                    e.Status != Domain.Enums.EnrollmentStatus.Cancelled)
                .Select(e => new CourseStudentDto
                {
                    StudentName = e.Student.UserName,
                    Email = e.Student.Email,
                    Progress = e.ProgressPercentage,
                    EnrolledAt = e.EnrolledAt
                })
                .ToListAsync(cancellationToken);

            return Result<List<CourseStudentDto>>.Success(students);
        }
    }
}
