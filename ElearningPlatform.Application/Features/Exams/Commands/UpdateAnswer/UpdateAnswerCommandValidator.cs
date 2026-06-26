using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Commands.UpdateAnswer
{
    public class UpdateAnswerCommandValidator
        : AbstractValidator<UpdateAnswerCommand>
    {
        public UpdateAnswerCommandValidator()
        {
            RuleFor(x => x.AttemptId)
                .GreaterThan(0);

            RuleFor(x => x.QuestionId)
                .GreaterThan(0);

            RuleFor(x => x)
                .Must(x =>
                    x.SelectedAnswerId.HasValue ||
                    !string.IsNullOrWhiteSpace(x.TextAnswer))
                .WithMessage("Either SelectedAnswerId or TextAnswer must be provided.");
        }
    }
}
