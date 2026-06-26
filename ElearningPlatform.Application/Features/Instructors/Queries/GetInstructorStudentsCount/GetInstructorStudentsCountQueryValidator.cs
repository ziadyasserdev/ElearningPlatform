using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetInstructorStudentsCount
{
    public class GetInstructorStudentsCountQueryValidator : AbstractValidator<GetInstructorStudentsCountQuery>
    {
        public GetInstructorStudentsCountQueryValidator()
        {
            RuleFor(x => x.InstructorId)
                .GreaterThan(0)
                .WithMessage("InstructorId must be greater than zero.");
        }
    }
}
