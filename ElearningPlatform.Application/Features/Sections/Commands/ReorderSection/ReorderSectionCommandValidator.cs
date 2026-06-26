using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Sections.Commands.ReorderSection
{
    public class ReorderSectionCommandValidator : AbstractValidator<ReorderSectionCommand>
    {
        public ReorderSectionCommandValidator()
        {
            RuleFor(x => x.SectionId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Section Id is required")
                .GreaterThan(0).WithMessage("Section Id must be greater than 0");

            RuleFor(x => x.NewOrder)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("New order is required")
                .GreaterThan(0).WithMessage("New order must be greater than 0");
        }
    }
}
