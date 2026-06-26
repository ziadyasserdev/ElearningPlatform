using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Commands.EditRole
{
    public class EditRoleCommandValidator : AbstractValidator<EditRoleCommand>
    {


        public EditRoleCommandValidator()
        {


            RuleFor(x => x.roleId)
                .NotEmpty().WithMessage("Role Id is required.");

            RuleFor(x => x.newRoleName)
                .NotEmpty().WithMessage("Role name is required.")
                .MinimumLength(3).WithMessage("Role name must be at least 3 characters.")
                .MaximumLength(20).WithMessage("Role name must not exceed 20 characters.")
               ;
        }
    }
}
