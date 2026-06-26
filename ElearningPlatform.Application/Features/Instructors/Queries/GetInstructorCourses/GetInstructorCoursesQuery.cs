using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Instructors.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetInstructorCourses
{
    public class GetInstructorCoursesQuery:IRequest<Result<List<InstructorCourseDto>>>
    {
        public int InstructorId { get; set; }
        public GetInstructorCoursesQuery(int instructorId)
        {
            InstructorId = instructorId;
        }
    }
}
