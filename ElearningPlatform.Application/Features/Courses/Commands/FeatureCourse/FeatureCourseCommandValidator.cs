using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Commands.FeatureCourse
{
    public class FeatureCourseCommandValidator
       : AbstractValidator<FeatureCourseCommand>
    {
        public FeatureCourseCommandValidator()
        {
            RuleFor(x => x.CourseId)
                .GreaterThan(0)
                .WithMessage("CourseId must be greater than zero.");
        }
    }

}
