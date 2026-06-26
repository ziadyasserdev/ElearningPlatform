using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Lessons.Commands.ToggleLessonPreview
{
    public class ToggleLessonPreviewCommandValidator : AbstractValidator<ToggleLessonPreviewCommand>
    {
        public ToggleLessonPreviewCommandValidator()
        {
            RuleFor(x => x.LessonId)
                .GreaterThan(0)
                .WithMessage("LessonId must be greater than 0");
        }
    }
}
