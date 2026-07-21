using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Queries.ExportSubmissions
{
    public class ExportSubmissionsQueryValidator
       : AbstractValidator<ExportSubmissionsQuery>
    {
        public ExportSubmissionsQueryValidator()
        {
            RuleFor(x => x.AssignmentId)
                .GreaterThan(0)
                .WithMessage("Assignment Id must be greater than zero.");
        }
    }
}
