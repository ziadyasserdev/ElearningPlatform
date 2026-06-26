using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Queries.GetVideosByLessonId
{
    public class GetVideosByLessonIdQueryValidator : AbstractValidator<GetVideosByLessonIdQuery>
    {
        public GetVideosByLessonIdQueryValidator()
        {
            RuleFor(x => x.LessonId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Lesson Id is required")
                .GreaterThan(0).WithMessage("Lesson Id must be greater than 0");
        }
    }
}
