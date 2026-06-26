using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Questions.Commands.CreateAnswer
{
    public class CreateAnswerCommandValidator
           : AbstractValidator<CreateAnswerCommand>
    {
        public CreateAnswerCommandValidator()
        {
            RuleFor(x => x.QuestionId)
                .GreaterThan(0)
                .WithMessage("Question Id must be greater than 0.");

            RuleFor(x => x.AnswerText)
                .NotEmpty()
                .MaximumLength(500)
                .WithMessage("Answer text is required.");
        }
    }
}
