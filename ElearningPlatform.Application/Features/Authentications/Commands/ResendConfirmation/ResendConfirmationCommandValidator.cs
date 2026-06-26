using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authentications.Commands.ResendConfirmation
{
    public class ResendConfirmationCommandValidator
      : AbstractValidator<ResendConfirmationCommand>
    {
        public ResendConfirmationCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.")
                .MaximumLength(256);
        }
    }
}
