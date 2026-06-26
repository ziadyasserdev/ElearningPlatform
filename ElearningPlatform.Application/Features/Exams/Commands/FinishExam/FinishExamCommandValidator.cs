using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Commands.FinishExam
{
    public class FinishExamCommandValidator
         : AbstractValidator<FinishExamCommand>
    {
        public FinishExamCommandValidator()
        {
            RuleFor(x => x.AttemptId)
                .GreaterThan(0)
                .WithMessage("Attempt Id must be greater than 0.");
        }
    }
}
