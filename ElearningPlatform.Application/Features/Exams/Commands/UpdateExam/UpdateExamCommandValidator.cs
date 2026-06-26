using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Commands.UpdateExam
{
    public class UpdateExamCommandValidator : AbstractValidator<UpdateExamCommand>
    {
        public UpdateExamCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(1000);

            RuleFor(x => x.DurationMinutes)
                .GreaterThan(0)
                .LessThanOrEqualTo(300);

            RuleFor(x => x.TotalScore)
                .GreaterThan(0);

            RuleFor(x => x.PassingScore)
                .GreaterThan(0)
                .LessThanOrEqualTo(x => x.TotalScore);

        }
    }
}
