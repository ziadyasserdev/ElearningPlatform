using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Queriess.GetMyExamResult
{
    public class GetMyExamResultQueryValidator
       : AbstractValidator<GetMyExamResultQuery>
    {
        public GetMyExamResultQueryValidator()
        {
            RuleFor(x => x.AttemptId)
                .GreaterThan(0)
                .WithMessage("Attempt Id must be greater than 0.");
        }
    }
}
