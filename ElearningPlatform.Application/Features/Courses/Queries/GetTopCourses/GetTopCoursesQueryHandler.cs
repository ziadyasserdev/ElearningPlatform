using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Queries.GetTopCourses
{
    public class GetTopCoursesQueryHandler
       : IRequestHandler<GetTopCoursesQuery, Result<List<TopCourseDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetTopCoursesQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<List<TopCourseDto>>> Handle(
            GetTopCoursesQuery request,
            CancellationToken cancellationToken)
        {
            var enrollments = await unitOfWork.Enrollments.Query()
                .Include(e => e.Course)
                    .ThenInclude(c => c.Instructor)
                    .ThenInclude(c => c.User)
                .Where(e => e.Status != EnrollmentStatus.Cancelled)
                .ToListAsync(cancellationToken);

            var result = enrollments
                .GroupBy(e => new
                {
                    e.CourseId,
                    e.Course.Title,
                    InstructorName = e.Course.Instructor.User.FullName
                })
                .Select(g => new TopCourseDto
                {
                    CourseId = g.Key.CourseId,
                    CourseTitle = g.Key.Title,
                    InstructorName = g.Key.InstructorName,

                    TotalEnrollments = g.Count(),

                    ActiveEnrollments = g.Count(x =>
                        x.Status == EnrollmentStatus.Active),

                    CompletedEnrollments = g.Count(x =>
                        x.Status == EnrollmentStatus.Completed)
                })
                .OrderByDescending(x => x.TotalEnrollments)
                .Take(request.Take)
                .ToList();

            return Result<List<TopCourseDto>>.Success(result);
        }
    }
}
