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

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetInstructorCourses
{
    public class GetInstructorCoursesQueryHandler : IRequestHandler<GetInstructorCoursesQuery, Result<List<InstructorCourseDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetInstructorCoursesQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<InstructorCourseDto>>> Handle(GetInstructorCoursesQuery request, CancellationToken cancellationToken)
        {
            var instructorExists = await unitOfWork.Instructors.Query()
                .AsNoTracking()
                .AnyAsync(i => i.Id == request.InstructorId, cancellationToken);

            if(!instructorExists)
                return Result<List<InstructorCourseDto>>.Failure(ResultStatus.NotFound,"Instructor not found");
            
            var courses = await unitOfWork.Courses.Query()
               .AsNoTracking()
               .Where(c => c.InstructorId == request.InstructorId)
               .Select(c => new InstructorCourseDto
               {
                   Id = c.Id,
                   Title = c.Title,
                   ThumbnailUrl = c.ThumbnailUrl!,
                   Price = c.Price,
                   StudentsCount = c.Enrollments.Count,
                   Rating = c.AverageRating
               })
               .ToListAsync(cancellationToken);

            return Result<List<InstructorCourseDto>>.Success(courses);

        }
    }
}
