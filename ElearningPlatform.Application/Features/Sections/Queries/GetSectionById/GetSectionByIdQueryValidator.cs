using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Sections.Queries.GetSectionById
{
    public class GetSectionByIdQueryValidator : AbstractValidator<GetSectionByIdQuery>
    {
        public GetSectionByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Section Id must be greater than 0");
        }
    }
}
