using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Lessons.Queries.GetLessonsBySection
{
    public class GetLessonsBySectionQueryValidator
        : AbstractValidator<GetLessonsBySectionQuery>
    {
        public GetLessonsBySectionQueryValidator()
        {
            RuleFor(x => x.SectionId)
                .GreaterThan(0)
                .WithMessage("SectionId must be greater than 0");
        }
    }
}
