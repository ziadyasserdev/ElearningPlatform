using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Queries.IsRoleExist
{
    public class IsRoleExistQueryValidator : AbstractValidator<IsRoleExistQuery>
    {
        public IsRoleExistQueryValidator()
        {
            RuleFor(x => x.RoleId)
                .NotEmpty()
                .WithMessage("RoleId must be provided.")
                .MaximumLength(450)
                .WithMessage("RoleId is too long.");
        }
    }
}
