using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Commands.ResubmitSubmission
{

    public class ResubmitSubmissionCommandValidator
        : AbstractValidator<ResubmitSubmissionCommand>
    {
        public ResubmitSubmissionCommandValidator()
        {
            RuleFor(x => x.SubmissionId)
                .GreaterThan(0);

            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("Submission file is required.");
        }
    }
}
