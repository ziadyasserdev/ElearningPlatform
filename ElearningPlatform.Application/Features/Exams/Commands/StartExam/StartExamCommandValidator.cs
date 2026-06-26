using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Commands.StartExam
{
    public class StartExamCommandValidator
        : AbstractValidator<StartExamCommand>
    {
        public StartExamCommandValidator()
        {
            RuleFor(x => x.ExamId)
                .GreaterThan(0)
                .WithMessage("Exam Id must be greater than 0.");
        }
    }
}
