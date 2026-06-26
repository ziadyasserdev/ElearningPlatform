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

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetInactiveStudents
{
    public class GetInactiveStudentsQueryHandler
    : IRequestHandler<GetInactiveStudentsQuery, Result<List<InactiveStudentDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetInactiveStudentsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<InactiveStudentDto>>> Handle(
            GetInactiveStudentsQuery request,
            CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<List<InactiveStudentDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required");
            var instructorId = currentUserService.UserId;
            var cutoffDate = DateTime.UtcNow.AddDays(-30);

            var course = await unitOfWork.Courses.Query()
                .FirstOrDefaultAsync(c =>
                    c.Id == request.CourseId &&
                    c.Instructor.UserId == instructorId,
                    cancellationToken);

            if (course is null)
                return Result<List<InactiveStudentDto>>.Failure(
                    ResultStatus.NotFound,
                    "Course not found");

            var enrollments = await unitOfWork.Enrollments.Query()
                .Include(e => e.Student)
                .Where(e =>
                    e.CourseId == request.CourseId &&
                    e.Status != EnrollmentStatus.Cancelled)
                .ToListAsync(cancellationToken);

            var videoProgress = await unitOfWork.VideoProgresses.Query()
                .Include(v => v.Video)
                .Where(v =>
                    v.Video.Lesson.Section.CourseId == request.CourseId &&
                    v.UserId != null)
                .ToListAsync(cancellationToken);

            var result = enrollments
                .Select(e =>
                {
                    var lastActivity = videoProgress
                        .Where(v => v.UserId == e.StudentId)
                        .OrderByDescending(v => v.LastWatchedAt)
                        .FirstOrDefault();

                    var lastActiveAt = lastActivity?.LastWatchedAt;

                    var isInactive =
                        lastActiveAt == null ||
                        lastActiveAt < cutoffDate;

                    if (!isInactive)
                        return null;

                    return new InactiveStudentDto
                    {
                        StudentId = e.StudentId,
                        StudentName = e.Student.FullName,
                        Email = e.Student.Email,
                        EnrolledAt = e.EnrolledAt,
                        LastActiveAt = lastActiveAt,
                        ProgressPercentage = e.ProgressPercentage
                    };
                })
                .Where(x => x != null)
                .ToList();

            return Result<List<InactiveStudentDto>>.Success(result!);
        }
    }
}
