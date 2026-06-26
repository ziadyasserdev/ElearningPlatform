using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetAllInstructorsWithPagination
{
    public class GetAllInstructorsWithPaginationQueryValidator
       : AbstractValidator<GetAllInstructorsWithPaginationQuery>
    {
        public GetAllInstructorsWithPaginationQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .NotEmpty()
                .WithMessage("Page number is required")
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0");

            RuleFor(x => x.PageSize)
                .NotEmpty()
                .WithMessage("Page size is required")
                .GreaterThan(0)
                .WithMessage("Page size must be greater than 0")
                .LessThanOrEqualTo(100)
                .WithMessage("Page size cannot be greater than 100");
        }
    }
}
