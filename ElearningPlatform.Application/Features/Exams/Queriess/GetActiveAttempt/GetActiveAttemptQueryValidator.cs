using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Queriess.GetActiveAttempt
{
    public class GetActiveAttemptQueryValidator
          : AbstractValidator<GetActiveAttemptQuery>
    {
        public GetActiveAttemptQueryValidator()
        {
            RuleFor(x => x.ExamId)
                .GreaterThan(0)
                .WithMessage("Exam Id must be greater than 0.");
        }
    }
}
