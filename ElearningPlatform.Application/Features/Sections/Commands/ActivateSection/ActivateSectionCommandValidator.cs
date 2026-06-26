using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Sections.Commands.ActivateSection
{
    public class ActivateSectionCommandValidator : AbstractValidator<ActivateSectionCommand>
    {
        public ActivateSectionCommandValidator()
        {
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Section Id is required")
                .GreaterThan(0).WithMessage("Section Id must be greater than 0");
        }
    }
}
