using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Questions.Queries.GetExamQuestions
{
    public class GetExamQuestionsQueryValidator
        : AbstractValidator<GetExamQuestionsQuery>
    {
        public GetExamQuestionsQueryValidator()
        {
            RuleFor(x => x.ExamId)
                .GreaterThan(0)
                .WithMessage("Exam Id must be greater than 0.");
        }
    }
}
