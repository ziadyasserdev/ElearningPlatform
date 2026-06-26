using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Commands.RemoveClaimFromUser
{
    public class RemoveClaimFromUserCommandValidator : AbstractValidator<RemoveClaimFromUserCommand>
    {
        public RemoveClaimFromUserCommandValidator()
        {
            RuleFor(x => x.userId)
                .NotEmpty()
                .WithMessage("User ID must be provided.");

            RuleFor(x => x.claimType)
                .NotEmpty()
                .WithMessage("Claim type must be provided.");

            RuleFor(x => x.claimValue)
                .NotEmpty()
                .WithMessage("Claim value must be provided.");
        }
    }
}
