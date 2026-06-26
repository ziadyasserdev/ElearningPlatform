using ElearningPlatform.Application.Features.Exams.Commands.UpdateAnswer;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Questions.Commands.UpdateAnswer
{
    public class UpdateQuestionAnswerCommandValidator
        : AbstractValidator<UpdateQuestionAnswerCommand>
    {
        public UpdateQuestionAnswerCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Answer Id must be greater than 0.");

            RuleFor(x => x.AnswerText)
                .NotEmpty()
                .MaximumLength(500)
                .WithMessage("Answer text is required.");
        }
    }
}
