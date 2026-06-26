using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Lessons.Commands.PublishLesson
{
    public class PublishLessonCommandValidator:AbstractValidator<PublishLessonCommand>
    {
        public PublishLessonCommandValidator()
        {
            RuleFor(x => x.Id)
              .NotEmpty().WithMessage("Lesson Id is required")
              .GreaterThan(0).WithMessage("Lesson Id must be greater than 0");
        }
    }
}
