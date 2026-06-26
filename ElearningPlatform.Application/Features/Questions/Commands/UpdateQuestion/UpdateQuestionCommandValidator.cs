using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Questions.Commands.UpdateQuestion
{
    public class UpdateQuestionCommandValidator
       : AbstractValidator<UpdateQuestionCommand>
    {
        public UpdateQuestionCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.QuestionText)
                .NotEmpty()
                .MaximumLength(1000);

            RuleFor(x => x.QuestionType)
                .IsInEnum();

            RuleFor(x => x.Score)
                .GreaterThan(0)
                .WithMessage("Score must be greater than zero.");
        }
    }
}
