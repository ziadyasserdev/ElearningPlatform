using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Queries.GetAllRolesOfUser
{
    public class GetAllRolesOfUserQueryValidator : AbstractValidator<GetAllRolesOfUserQuery>
    {
        public GetAllRolesOfUserQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID must be provided.");
        }
    }
}
