using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Questions.Commands.CreateQuestion
{
    public class CreateQuestionCommandValidator
      : AbstractValidator<CreateQuestionCommand>
    {
        public CreateQuestionCommandValidator()
        {
            RuleFor(x => x.QuestionText)
                .NotEmpty()
                .MaximumLength(1000);

            RuleFor(x => x.Score)
                .GreaterThan(0);

            RuleFor(x => x.QuestionType)
                .IsInEnum();
        }
    }
}
