using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Queries.GetTopStudents
{
    public class GetAssignmentTopStudentsQueryValidator
      : AbstractValidator<GetAssignmentTopStudentsQuery>
    {
        public GetAssignmentTopStudentsQueryValidator()
        {
            RuleFor(x => x.AssignmentId)
                .GreaterThan(0)
                .WithMessage("Assignment Id must be greater than zero.");

            RuleFor(x => x.Count)
                .InclusiveBetween(1, 100)
                .WithMessage("Count must be between 1 and 100.");
        }
    }
}
