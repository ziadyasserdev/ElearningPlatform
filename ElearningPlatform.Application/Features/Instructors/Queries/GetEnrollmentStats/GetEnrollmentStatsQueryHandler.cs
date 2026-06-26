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

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetEnrollmentStats
{
    public class GetEnrollmentStatsQueryHandler
       : IRequestHandler<GetEnrollmentStatsQuery, Result<EnrollmentStatsDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetEnrollmentStatsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<EnrollmentStatsDto>> Handle(
            GetEnrollmentStatsQuery request,
            CancellationToken cancellationToken)
        {
            var instructorId = currentUserService.UserId;

            var course = await unitOfWork.Courses.Query()
                
                .FirstOrDefaultAsync(c =>
                    c.Id == request.CourseId &&
                    c.Instructor.UserId == instructorId,
                    cancellationToken);

            if (course is null)
                return Result<EnrollmentStatsDto>.Failure(
                    ResultStatus.NotFound,
                    "Course not found");

            var enrollments = await unitOfWork.Enrollments.Query()
                .Where(e =>
                    e.CourseId == request.CourseId &&
                    e.Status != EnrollmentStatus.Cancelled)
                .ToListAsync(cancellationToken);

            var totalStudents = enrollments.Count;

            var completedStudents = enrollments
                .Count(e => e.Status == EnrollmentStatus.Completed);

            var completionRate =
                totalStudents == 0
                    ? 0
                    : (double)completedStudents / totalStudents * 100;

            var result = new EnrollmentStatsDto
            {
                TotalStudents = totalStudents,
                CompletedStudents = completedStudents,
                CompletionRate = Math.Round(completionRate, 2)
            };

            return Result<EnrollmentStatsDto>.Success(result);
        }
    }

}
