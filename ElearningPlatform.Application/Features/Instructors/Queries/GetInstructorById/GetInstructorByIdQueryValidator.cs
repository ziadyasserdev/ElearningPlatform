using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetInstructorById
{
    public class GetInstructorByIdQueryValidator : AbstractValidator<GetInstructorByIdQuery>
    {
        public GetInstructorByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Instructor Id must be greater than 0");
        }
    }
}
