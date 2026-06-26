using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Queries.GetCourseDetails
{
    public class GetCourseDetailsQueryValidator : AbstractValidator<GetCourseDetailsQuery>
    {
        public GetCourseDetailsQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Course Id must be greater than 0");
        }
    }
}
