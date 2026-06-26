using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.ApplicationUsers.Queries.CheckEmailExist
{
    public class CheckEmailExistQueryValidator
     : AbstractValidator<CheckEmailExistQuery>
    {
        public CheckEmailExistQueryValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")

                .EmailAddress()
                .WithMessage("Invalid email format.")

                .MaximumLength(100)
                .WithMessage("Email must not exceed 100 characters.");
        }
    }
}
