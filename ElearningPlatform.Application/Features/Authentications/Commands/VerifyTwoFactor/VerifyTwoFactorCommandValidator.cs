using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authentications.Commands.VerifyTwoFactor
{
    public class VerifyTwoFactorCommandValidator : AbstractValidator<VerifyTwoFactorCommand>
    {
        public VerifyTwoFactorCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");

            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Verification code is required.")
                .Length(6)
                .WithMessage("Verification code must be 6 digits.")
                .Matches(@"^\d{6}$")
                .WithMessage("Verification code must contain only numbers.");
        }
    }
}
