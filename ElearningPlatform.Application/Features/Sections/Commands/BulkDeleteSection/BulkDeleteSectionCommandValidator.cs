using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Sections.Commands.BulkDeleteSection
{
    public class BulkDeleteSectionCommandValidator : AbstractValidator<BulkDeleteSectionCommand>
    {
        public BulkDeleteSectionCommandValidator()
        {
            RuleFor(x => x.SectionIds)
                .NotNull().WithMessage("SectionIds cannot be null")
                .NotEmpty().WithMessage("SectionIds cannot be empty");

            RuleForEach(x => x.SectionIds)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0).WithMessage("Each Section Id must be greater than 0");

            RuleFor(x => x.SectionIds)
                .Must(ids => ids.Distinct().Count() == ids.Count)
                .WithMessage("Duplicate Section Ids are not allowed");
        }
    }
}
