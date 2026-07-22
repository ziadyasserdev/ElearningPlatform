using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Queries.GetStudentEnrollments
{
    public class GetStudentEnrollmentsQueryValidator
    : AbstractValidator<GetStudentEnrollmentsQuery>
    {
        public GetStudentEnrollmentsQueryValidator()
        {
            RuleFor(x => x.StudentId)
                .NotEmpty()
                .WithMessage("Student Id is required.");

            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Page size must be between 1 and 100.");
        }
    }
}
