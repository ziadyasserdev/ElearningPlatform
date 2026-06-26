using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Commands.UpdateRoleOfUser
{
    public class UpdateRoleOfUserCommandValidator
     : AbstractValidator<UpdateRoleOfUserCommand>
    {
        public UpdateRoleOfUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required.");

            RuleFor(x => x.oldRoleId)
                .NotEmpty()
                .WithMessage("OldRoleId is required.");

            RuleFor(x => x.NewRoleId)
                .NotEmpty()
                .WithMessage("NewRoleId is required.");

            RuleFor(x => x)
                .Must(x => x.oldRoleId != x.NewRoleId)
                .WithMessage("OldRoleId and NewRoleId cannot be the same.");
        }
    }
}
