using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Lessons.Commands.BulkDeleteLessons
{
    public class BulkDeleteLessonsCommandValidator : AbstractValidator<BulkDeleteLessonsCommand>
    {
        public BulkDeleteLessonsCommandValidator()
        {
            RuleFor(x => x.LessonIds)
                .NotNull()
                .WithMessage("LessonIds cannot be null")
                .NotEmpty()
                .WithMessage("LessonIds cannot be empty");

            RuleFor(x => x.LessonIds)
                .Must(ids => ids.Distinct().Count() == ids.Count)
                .WithMessage("LessonIds must not contain duplicates");

            RuleForEach(x => x.LessonIds)
                .GreaterThan(0)
                .WithMessage("Each LessonId must be greater than 0");

         
            RuleFor(x => x.LessonIds.Count)
                .LessThanOrEqualTo(100)
                .WithMessage("Cannot delete more than 100 lessons at once");
        }
    }
}
