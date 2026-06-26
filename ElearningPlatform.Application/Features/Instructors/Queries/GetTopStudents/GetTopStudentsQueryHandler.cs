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

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetTopStudents
{
    public class GetTopStudentsQueryHandler
       : IRequestHandler<GetTopStudentsQuery, Result<List<TopStudentDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetTopStudentsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<TopStudentDto>>> Handle(
            GetTopStudentsQuery request,
            CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<List<TopStudentDto>>.Failure(ResultStatus.Unauthorized, "Authentication required");
            var instructorId = currentUserService.UserId;

            var course = await unitOfWork.Courses.Query()
                .FirstOrDefaultAsync(c =>
                    c.Id == request.CourseId &&
                    c.Instructor.UserId == instructorId,
                    cancellationToken);

            if (course is null)
                return Result<List<TopStudentDto>>.Failure(
                    ResultStatus.NotFound,
                    "Course not found");

            var enrollments = await unitOfWork.Enrollments.Query()
                .Include(e => e.Student)
                .Include(e => e.Course)
                .Where(e =>
                    e.CourseId == request.CourseId &&
                    e.Status != EnrollmentStatus.Cancelled)
                .ToListAsync(cancellationToken);

            var result = enrollments
                .Select(e =>
                {
                   

                    var progressScore = (double)e.ProgressPercentage;

                    var completionScore =
                        e.Status == EnrollmentStatus.Completed ? 100 : 0;

                    var videoEngagementScore =
                        progressScore * 0.6;

                    var enrollmentScore =
                        progressScore * 0.25;

                    var bonusScore =
                        completionScore * 0.15;

                    var totalScore =
                        videoEngagementScore +
                        enrollmentScore +
                        bonusScore;

                    return new TopStudentDto
                    {
                        StudentId = e.StudentId,
                        StudentName = e.Student.FullName,
                        Email = e.Student.Email,
                        ProgressPercentage = e.ProgressPercentage,
                        EnrolledAt = e.EnrolledAt,
                        CompletionScore = Math.Round(totalScore, 2)
                    };
                })
                .OrderByDescending(x => x.CompletionScore)
                .Take(10)
                .ToList();

            return Result<List<TopStudentDto>>.Success(result);
        }
    }
}
