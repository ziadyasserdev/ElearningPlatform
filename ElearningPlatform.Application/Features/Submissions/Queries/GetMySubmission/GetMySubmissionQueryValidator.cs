using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Queries.GetMySubmission
{
    public class GetMySubmissionQueryValidator
     : AbstractValidator<GetMySubmissionQuery>
    {
        public GetMySubmissionQueryValidator()
        {
            RuleFor(x => x.AssignmentId)
                .GreaterThan(0);
        }
    }
}
