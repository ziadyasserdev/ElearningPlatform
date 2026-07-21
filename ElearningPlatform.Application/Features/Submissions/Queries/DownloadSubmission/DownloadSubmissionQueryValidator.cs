using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Queries.DownloadSubmission
{
    public class DownloadSubmissionQueryValidator
       : AbstractValidator<DownloadSubmissionQuery>
    {
        public DownloadSubmissionQueryValidator()
        {
            RuleFor(x => x.SubmissionId)
                .GreaterThan(0)
                .WithMessage("Submission Id must be greater than zero.");
        }
    }
}
