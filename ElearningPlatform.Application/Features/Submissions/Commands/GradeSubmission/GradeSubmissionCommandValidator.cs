using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Commands.GradeSubmission
{
    public class GradeSubmissionCommandValidator
     : AbstractValidator<GradeSubmissionCommand>
    {
        public GradeSubmissionCommandValidator()
        {
            RuleFor(x => x.SubmissionId)
                .GreaterThan(0);

            RuleFor(x => x.Score)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Feedback)
                .MaximumLength(2000);
        }
    }
}
