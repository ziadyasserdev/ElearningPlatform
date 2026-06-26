using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Sections.Queries.GetSectionsCountByCourse
{
    public class GetSectionsCountByCourseQueryValidator
       : AbstractValidator<GetSectionsCountByCourseQuery>
    {
        public GetSectionsCountByCourseQueryValidator()
        {
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Course Id is required")
                .GreaterThan(0).WithMessage("Course Id must be greater than 0");
        }
    }
}
