using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Queries.GetAssignmentStatistics
{
    public class GetAssignmentStatisticsQueryValidator
       : AbstractValidator<GetAssignmentStatisticsQuery>
    {
        public GetAssignmentStatisticsQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Assignment Id must be greater than zero.");
        }
    }
}
