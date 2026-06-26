using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Instructors.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetInstructorDashboard
{
    public class GetInstructorDashboardQueryHandler : IRequestHandler<GetInstructorDashboardQuery, Result<InstructorDashboardDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetInstructorDashboardQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<InstructorDashboardDto>> Handle(GetInstructorDashboardQuery request, CancellationToken cancellationToken)
        {
            var instructor = await unitOfWork.Instructors.Query()
                .AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.User.Courses)
                    .ThenInclude(c => c.Enrollments)
                .Include(x => x.User.Reviews)
                .FirstOrDefaultAsync(x => x.Id == request.InstructorId, cancellationToken);

            if (instructor == null)
                return Result<InstructorDashboardDto>.Failure(ResultStatus.NotFound, "Instructor not found");

            var totalCourses = instructor.TotalCourses;

            var totalStudents = instructor.User.Courses
                .SelectMany(c => c.Enrollments)
                .Select(e => e.StudentId)
                .Distinct()
                .Count();

            var totalReviews = instructor.User.Reviews.Count;

            var averageRating = totalReviews > 0
                ? instructor.User.Reviews.Average(r => r.Rating)
                : 0;

            var dto = new InstructorDashboardDto
            {
                InstructorId = instructor.Id,
                FullName = instructor.User.FullName,
                TotalCourses = totalCourses,
                TotalStudents = totalStudents,
                TotalReviews = totalReviews,
                AverageRating = averageRating
            };

            return Result<InstructorDashboardDto>.Success(dto);
        }
    }
}
