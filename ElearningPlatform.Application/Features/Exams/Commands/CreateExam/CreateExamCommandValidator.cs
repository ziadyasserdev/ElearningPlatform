using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Commands.CreateExam
{
    public class CreateExamCommandValidator
       : AbstractValidator<CreateExamCommand>
    {
        public CreateExamCommandValidator()
        {
            RuleFor(x => x.CourseId)
                .GreaterThan(0);

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(2000);

            RuleFor(x => x.DurationMinutes)
                .GreaterThan(0);

            RuleFor(x => x.TotalScore)
                .GreaterThan(0);

            RuleFor(x => x.PassingScore)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x)
                .Must(x => x.PassingScore <= x.TotalScore)
                .WithMessage("Passing score cannot exceed total score");
        }
    }
}
