using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Queriess.GetExamAttempts
{
    public class GetExamAttemptsQueryValidator
       : AbstractValidator<GetExamAttemptsQuery>
    {
        public GetExamAttemptsQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Exam Id must be greater than 0");
        }
    }
}
