using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Commands.UpdateGrade
{
    public class UpdateGradeCommandValidator
           : AbstractValidator<UpdateGradeCommand>
    {
        public UpdateGradeCommandValidator()
        {
            RuleFor(x => x.SubmissionId)
                .GreaterThan(0)
                .WithMessage("Submission Id must be greater than zero.");

            RuleFor(x => x.Score)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Score must be greater than or equal to zero.");

            RuleFor(x => x.Feedback)
                .MaximumLength(2000)
                .When(x => !string.IsNullOrWhiteSpace(x.Feedback));
        }
    }
}
