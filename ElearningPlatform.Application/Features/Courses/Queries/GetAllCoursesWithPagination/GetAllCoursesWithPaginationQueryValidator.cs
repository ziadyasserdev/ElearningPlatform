using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Queries.GetAllCoursesWithPagination
{
    public class GetAllCoursesWithPaginationQueryValidator
       : AbstractValidator<GetAllCoursesWithPaginationQuery>
    {
        public GetAllCoursesWithPaginationQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("Page size must be greater than 0")
                .LessThanOrEqualTo(50)
                .WithMessage("Page size must not exceed 50");
        }
    }
}
