using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Enrollments.Dtos;
using ElearningPlatform.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Queries.GetStudentEnrollments
{
    public class GetStudentEnrollmentsQueryHandler
      : IRequestHandler<GetStudentEnrollmentsQuery,
          Result<PaginatedResult<StudentEnrollmentDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetStudentEnrollmentsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<PaginatedResult<StudentEnrollmentDto>>> Handle(
            GetStudentEnrollmentsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<PaginatedResult<StudentEnrollmentDto>>
                    .Failure(ResultStatus.Unauthorized,
                        "Authentication required.");
            }

            if (!currentUserService.IsInRole("Admin"))
            {
                return Result<PaginatedResult<StudentEnrollmentDto>>
                    .Failure(ResultStatus.Forbidden,
                        "Access denied.");
            }

            var studentExists = await unitOfWork.Users.Query()
                .AnyAsync(x => x.Id == request.StudentId, cancellationToken);

            if (!studentExists)
            {
                return Result<PaginatedResult<StudentEnrollmentDto>>
                    .Failure(ResultStatus.NotFound,
                        "Student not found.");
            }

            IQueryable<Enrollment> query = unitOfWork.Enrollments.Query()
                .AsNoTracking()
                .Include(x => x.Course)
                    .ThenInclude(x => x.Instructor)
                        .ThenInclude(x => x.User)
                .Where(x => x.StudentId == request.StudentId);

            var totalCount = await query.CountAsync(cancellationToken);

            var enrollments = await query
                .OrderByDescending(x => x.EnrolledAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new StudentEnrollmentDto
                {
                    EnrollmentId = x.Id,
                    CourseId = x.CourseId,
                    CourseTitle = x.Course.Title,
                    InstructorName = x.Course.Instructor.User.FullName,
                    ProgressPercentage = x.ProgressPercentage,
                    Status = x.Status.ToString(),
                    EnrolledAt = x.EnrolledAt,
                    CompletedAt = x.CompletedAt
                })
                .ToListAsync(cancellationToken);

            return Result<PaginatedResult<StudentEnrollmentDto>>
                .Success(new PaginatedResult<StudentEnrollmentDto>(enrollments,
                request.PageNumber,request.PageSize,totalCount));
               
        }
    }
}
