using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetTopInstructors
{
    public class GetTopInstructorsQueryValidator : AbstractValidator<GetTopInstructorsQuery>
    {
        public GetTopInstructorsQueryValidator()
        {
            RuleFor(x => x.Count)
                .GreaterThan(0)
                .LessThanOrEqualTo(50); 
        }
    }
}
