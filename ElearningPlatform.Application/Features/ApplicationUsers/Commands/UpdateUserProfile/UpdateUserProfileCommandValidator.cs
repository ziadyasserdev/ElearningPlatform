using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.ApplicationUsers.Commands.UpdateUserProfile
{
    public class UpdateUserProfileCommandValidator
       : AbstractValidator<UpdateUserProfileCommand>
    {
        public UpdateUserProfileCommandValidator()
        {

            RuleFor(x => x.FullName)
                .MaximumLength(100)
                .WithMessage("Full name must not exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.FullName));


            RuleFor(x => x.UserName)
                .MinimumLength(3)
                .WithMessage("Username must be at least 3 characters.")
                .MaximumLength(50)
                .WithMessage("Username must not exceed 50 characters.")
                .Matches("^[a-zA-Z0-9._-]*$")
                .WithMessage("Username can only contain letters, numbers, dots, underscores, and hyphens.")
                .When(x => !string.IsNullOrWhiteSpace(x.UserName));


            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Invalid email format.")
                .MaximumLength(256)
                .WithMessage("Email must not exceed 256 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));


            RuleFor(x => x.PhoneNumber)
                .Matches(@"^01[0-9]{9}$")
                .WithMessage("Invalid Egyptian phone number format.")
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));


            RuleFor(x => x.Gender)
                .IsInEnum()
                .WithMessage("Invalid gender value.")
                .When(x => x.Gender.HasValue);
        }
    }
}
