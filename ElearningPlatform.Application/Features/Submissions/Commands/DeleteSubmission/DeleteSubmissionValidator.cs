using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Commands.DeleteSubmission
{
    public class DeleteSubmissionCommandValidator
      : AbstractValidator<DeleteSubmissionCommand>
    {
        public DeleteSubmissionCommandValidator()
        {
            RuleFor(x => x.SubmissionId)
                .GreaterThan(0);
        }
    }
}
