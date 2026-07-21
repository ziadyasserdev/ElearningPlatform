using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Commands.ReturnSubmission
{
    public class ReturnSubmissionCommandValidator
        : AbstractValidator<ReturnSubmissionCommand>
    {
        public ReturnSubmissionCommandValidator()
        {
            RuleFor(x => x.SubmissionId)
                .GreaterThan(0);

            RuleFor(x => x.Reason)
                .NotEmpty()
                .MaximumLength(1000);
        }
    }
}
