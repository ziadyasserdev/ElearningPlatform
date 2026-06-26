using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Commands.AddClaimToUser
{
    public class AddClaimToUserCommandHandler : IRequestHandler<AddClaimToUserCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AddClaimToUserCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<Result<string>> Handle(AddClaimToUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.userId);
            if (user == null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    $"User with ID '{request.userId}' was not found."
                );
            var claim = new Claim(request.claimType, request.claimValue);
            var existingClaims = await userManager.GetClaimsAsync(user);
            if (existingClaims.Any(c => c.Type == request.claimType && c.Value == request.claimValue))
                return Result<string>.Failure(ResultStatus.Conflict, "User already has this claim.");
            var result = await userManager.AddClaimAsync(user, claim);
            if (!result.Succeeded)
            {
                var errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));

                return Result<string>.Failure(
                    ResultStatus.Failure,
                    $"Claim assignment failed. Details: {errorMessage}"
                );
            }

            return Result<string>.Success(
                user.Id,
                $"Claim '{claim.Value}' assigned to user successfully."
            );
        }
    }
}
