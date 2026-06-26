using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Commands.ToggleCategoryStatus
{
    public class ToggleCategoryStatusCommandValidator : AbstractValidator<ToggleCategoryStatusCommand>
    {
        public ToggleCategoryStatusCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Category Id must be greater than 0.");
        }
    }
}
