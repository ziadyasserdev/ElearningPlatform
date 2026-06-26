using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Commands.RemoveRoleFromUser
{
    public class RemoveRoleFromUserCommandValidator : AbstractValidator<RemoveRoleFromUserCommand>
    {
        public RemoveRoleFromUserCommandValidator()
        {
            RuleFor(x => x.userId)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(x => x.roleId)
                .NotEmpty().WithMessage("Role ID is required.");
        }
    }
}
