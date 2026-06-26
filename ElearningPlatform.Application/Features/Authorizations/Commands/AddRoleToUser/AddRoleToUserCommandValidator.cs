using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Commands.AddRoleToUser
{
    public class AddRoleToUserCommandValidator
        : AbstractValidator<AddRoleToUserCommand>
    {
        public AddRoleToUserCommandValidator()
        {
            RuleFor(x => x.userId)
                .NotEmpty()
                .WithMessage("UserId is required.");

            RuleFor(x => x.roleId)
                .NotEmpty()
                .WithMessage("RoleId is required.");
        }
    }
}
