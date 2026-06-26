using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Commands.RemoveCategoryIcon
{
    public class RemoveCategoryIconCommandValidator : AbstractValidator<RemoveCategoryIconCommand>
    {
        public RemoveCategoryIconCommandValidator()
        {
            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage("CategoryId must be greater than 0.");
        }
    }
}
