using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetInstructorCourses
{
    public class GetInstructorCoursesQueryValidator : AbstractValidator<GetInstructorCoursesQuery>
    {
        public GetInstructorCoursesQueryValidator()
        {
            RuleFor(x => x.InstructorId)
                .GreaterThan(0);
        }
    }
}
