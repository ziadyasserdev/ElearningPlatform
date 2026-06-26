using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Commands.EditUserClaimValue
{
    public class EditUserClaimValueCommandValidator : AbstractValidator<EditUserClaimValueCommand>
    {
        public EditUserClaimValueCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
            RuleFor(x => x.ClaimType).NotEmpty().WithMessage("Claim type is required.");
            RuleFor(x => x.NewClaimValue).NotEmpty().WithMessage("New claim value is required.");
        }
    }
}
