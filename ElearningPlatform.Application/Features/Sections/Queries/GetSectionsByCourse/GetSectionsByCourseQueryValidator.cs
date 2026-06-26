using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Sections.Queries.GetSectionsByCourse
{
    public class GetSectionsByCourseQueryValidator : AbstractValidator<GetSectionsByCourseQuery>
    {
        public GetSectionsByCourseQueryValidator()
        {
            RuleFor(x => x.CourseId)
                .GreaterThan(0)
                .WithMessage("CourseId must be greater than 0");
        }
    }
}
