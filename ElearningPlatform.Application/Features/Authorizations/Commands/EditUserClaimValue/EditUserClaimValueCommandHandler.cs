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

namespace ElearningPlatform.Application.Features.Authorizations.Commands.EditUserClaimValue
{
    public class EditUserClaimValueCommandHandler : IRequestHandler<EditUserClaimValueCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        
        public EditUserClaimValueCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager
        )
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<Result<string>> Handle(EditUserClaimValueCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    $"User with ID '{request.UserId}' was not found."
                );
            var existingClaims = await userManager.GetClaimsAsync(user);


            var oldClaim = existingClaims.FirstOrDefault(x =>
                x.Type == request.ClaimType && x.Value == request.oldClaimValue);

            if (oldClaim == null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    $"Claim '{request.ClaimType}' with value '{request.oldClaimValue}' does not exist for this user."
                );


            if (existingClaims.Any(x =>
                x.Type == request.ClaimType && x.Value == request.NewClaimValue))
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    $"User already has a claim with type '{request.ClaimType}' and value '{request.NewClaimValue}'."
                );
            }


            var removeResult = await userManager.RemoveClaimAsync(user, oldClaim);
            if (!removeResult.Succeeded)
            {
                var errorMessage = string.Join(" | ", removeResult.Errors.Select(e => e.Description));
                return Result<string>.Failure(ResultStatus.Failure, $"Failed to remove old claim: {errorMessage}");
            }


            var newClaim = new Claim(request.ClaimType, request.NewClaimValue);
            var addResult = await userManager.AddClaimAsync(user, newClaim);
            if (!addResult.Succeeded)
            {
                var errorMessage = string.Join(" | ", addResult.Errors.Select(e => e.Description));
                return Result<string>.Failure(ResultStatus.Failure, $"Failed to add new claim: {errorMessage}");
            }


            return Result<string>.Success(user.Id);
        }
    }
}
