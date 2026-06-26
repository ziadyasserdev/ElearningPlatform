using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Commands.AddClaimToUser
{
    public class AddClaimToUserCommandValidator : AbstractValidator<AddClaimToUserCommand>
    {
        public AddClaimToUserCommandValidator()
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
