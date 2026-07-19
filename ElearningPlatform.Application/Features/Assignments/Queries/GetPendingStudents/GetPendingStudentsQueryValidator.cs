using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Queries.GetPendingStudents
{
    public class GetPendingStudentsQueryValidator
          : AbstractValidator<GetPendingStudentsQuery>
    {
        public GetPendingStudentsQueryValidator()
        {
            RuleFor(x => x.AssignmentId)
                .GreaterThan(0);

            RuleFor(x => x.PageNumber)
                .GreaterThan(0);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100);
        }
    }
}
