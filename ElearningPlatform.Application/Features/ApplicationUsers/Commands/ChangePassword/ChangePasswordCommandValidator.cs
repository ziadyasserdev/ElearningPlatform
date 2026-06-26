using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.ApplicationUsers.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.oldPassword)
                .NotEmpty()
                .WithMessage("Old password is required.");

            RuleFor(x => x.newPassword)
                .NotEmpty()
                .WithMessage("New password is required.")
                .MinimumLength(6)
                .WithMessage("New password must be at least 6 characters.")
                .NotEqual(x => x.oldPassword)
                .WithMessage("New password must be different from old password.");

            RuleFor(x => x.confirmedPassword)
                .NotEmpty()
                .WithMessage("Confirm password is required.")
                .Equal(x => x.newPassword)
                .WithMessage("Confirm password must match the new password.");
        }
    }
}
