using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Questions.Queries.GetQuestionAnswers
{
    public class GetQuestionAnswersQueryValidator
      : AbstractValidator<GetQuestionAnswersQuery>
    {
        public GetQuestionAnswersQueryValidator()
        {
            RuleFor(x => x.QuestionId)
                .GreaterThan(0)
                .WithMessage("Question Id must be greater than 0.");
        }
    }
}
