using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Enrollments.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Queries.GetCourseStudents
{
    public class GetCourseStudentsQueryHandler
       : IRequestHandler<GetCourseStudentsQuery,
           Result<PaginatedResult<CourseStudentDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetCourseStudentsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<PaginatedResult<CourseStudentDto>>> Handle(
            GetCourseStudentsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<PaginatedResult<CourseStudentDto>>
                    .Failure(ResultStatus.Unauthorized,
                        "Authentication required.");
            }

            var course = await unitOfWork.Courses.Query()
                .Include(x => x.Instructor)
                .FirstOrDefaultAsync(x =>
                    x.Id == request.CourseId &&
                    !x.IsDeleted,
                    cancellationToken);

            if (course == null)
            {
                return Result<PaginatedResult<CourseStudentDto>>
                    .Failure(ResultStatus.NotFound,
                        "Course not found.");
            }

            if (currentUserService.IsInRole("Instructor"))
            {
                if (course.Instructor.UserId != currentUserService.UserId)
                {
                    return Result<PaginatedResult<CourseStudentDto>>
                        .Failure(ResultStatus.Forbidden,
                            "You are not allowed to access this course.");
                }
            }
            else if (!currentUserService.IsInRole("Admin"))
            {
                return Result<PaginatedResult<CourseStudentDto>>
                    .Failure(ResultStatus.Forbidden,
                        "Access denied.");
            }

            var query = unitOfWork.Enrollments.Query()
                .AsNoTracking()
                .Include(x => x.Student)
                .Where(x => x.CourseId == request.CourseId);
                

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x =>
                    x.Student.FullName.Contains(request.Search) ||
                    x.Student.Email!.Contains(request.Search));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var students = await query
                .OrderByDescending(x => x.EnrolledAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new CourseStudentDto
                {
                    StudentId = x.StudentId,
                    StudentName = x.Student.FullName,
                    StudentEmail = x.Student.Email!,
                    ProgressPercentage = x.ProgressPercentage,
                    Status = x.Status.ToString(),
                    EnrolledAt = x.EnrolledAt
                })
                .ToListAsync(cancellationToken);

            return Result<PaginatedResult<CourseStudentDto>>
                .Success(new PaginatedResult<CourseStudentDto>(students,
                request.PageNumber,request.PageSize,totalCount));
               
        }
    }
}
