using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Commands.CreateSubmission
{
    public class CreateSubmissionCommandValidator
       : AbstractValidator<CreateSubmissionCommand>
    {
        public CreateSubmissionCommandValidator()
        {
            RuleFor(x => x.AssignmentId)
                .GreaterThan(0)
                .WithMessage("Assignment Id must be greater than zero.");

            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("Submission file is required.");

            RuleFor(x => x.File.Length)
                .GreaterThan(0)
                .When(x => x.File != null)
                .WithMessage("Submission file cannot be empty.");

            RuleFor(x => x.File.Length)
                .LessThanOrEqualTo(20 * 1024 * 1024)
                .When(x => x.File != null)
                .WithMessage("File size must not exceed 20 MB.");

            RuleFor(x => x.Comment)
                .MaximumLength(1000)
                .When(x => !string.IsNullOrWhiteSpace(x.Comment));
        }
    }
}
