using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Assignments.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Queries.GetPendingStudents
{
    public class GetPendingStudentsQueryHandler
       : IRequestHandler<GetPendingStudentsQuery,
           Result<PaginatedResult<PendingStudentDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetPendingStudentsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<PaginatedResult<PendingStudentDto>>> Handle(
            GetPendingStudentsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<PaginatedResult<PendingStudentDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

            var userId = currentUserService.UserId;

            var assignment = await unitOfWork.Assignments
                .Query()
                .Include(a => a.Course)
                .ThenInclude(c => c.Instructor)
                .FirstOrDefaultAsync(a =>
                    a.Id == request.AssignmentId &&
                    !a.IsDeleted,
                    cancellationToken);

            if (assignment is null)
            {
                return Result<PaginatedResult<PendingStudentDto>>.Failure(
                    ResultStatus.NotFound,
                    "Assignment not found.");
            }

            if (assignment.Course.Instructor.UserId != userId)
            {
                return Result<PaginatedResult<PendingStudentDto>>.Failure(
                    ResultStatus.Forbidden,
                    "You are not authorized to view pending students.");
            }

            var query = unitOfWork.Enrollments
                .Query()
                .Where(e =>
                    e.CourseId == assignment.CourseId &&
                    !e.IsDeleted &&
                    !unitOfWork.Submissions
                        .Query()
                        .Any(s =>
                            s.AssignmentId == assignment.Id &&
                            s.StudentId == e.StudentId &&
                            !s.IsDeleted));

            var totalCount = await query.CountAsync(cancellationToken);

            var students = await query
                .OrderBy(e => e.Student.FullName)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(e => new PendingStudentDto
                {
                    StudentId = e.StudentId,
                    StudentName = e.Student.FullName,
                    ProfilePictureUrl = e.Student.ProfileImageUrl,
                    EnrolledAt = e.EnrolledAt,
                    ProgressPercentage = e.ProgressPercentage
                })
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<PendingStudentDto>(
                students,
                request.PageNumber,
                request.PageSize,
                totalCount);

            return Result<PaginatedResult<PendingStudentDto>>
                .Success(result);
        }
    }
}
