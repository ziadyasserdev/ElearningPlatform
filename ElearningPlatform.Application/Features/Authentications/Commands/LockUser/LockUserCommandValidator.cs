using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authentications.Commands.LockUser
{
    public class LockUserCommandValidator : AbstractValidator<LockUserCommand>
    {
        public LockUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .MinimumLength(10).WithMessage("UserId seems too short.")
                .MaximumLength(100).WithMessage("UserId seems too long.");
        }
    }
}
