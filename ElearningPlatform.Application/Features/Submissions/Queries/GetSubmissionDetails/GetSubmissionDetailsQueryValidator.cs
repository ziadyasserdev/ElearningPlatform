using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Queries.GetSubmissionDetails
{
    public class GetSubmissionDetailsQueryValidator
       : AbstractValidator<GetSubmissionDetailsQuery>
    {
        public GetSubmissionDetailsQueryValidator()
        {
            RuleFor(x => x.SubmissionId)
                .GreaterThan(0);
        }
    }
}
