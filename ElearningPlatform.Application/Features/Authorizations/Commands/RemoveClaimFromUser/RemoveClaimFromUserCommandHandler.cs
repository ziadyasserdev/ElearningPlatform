using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Commands.RemoveClaimFromUser
{
    public class RemoveClaimFromUserCommandHandler : IRequestHandler<RemoveClaimFromUserCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public RemoveClaimFromUserCommandHandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Result<string>> Handle(RemoveClaimFromUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.userId);
            if (user == null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    $"User with ID '{request.userId}' was not found."
                );
            var userCliams = await userManager.GetClaimsAsync(user);
            var oldClaim = userCliams.FirstOrDefault(x => x.Type == request.claimType && x.Value == request.claimValue);
            if (oldClaim == null)
                return Result<string>.Failure(
                         ResultStatus.NotFound,
                         $"Claim '{request.claimType}' with value '{request.claimValue}' does not exist for this user."
                     );
            var result = await userManager.RemoveClaimAsync(user, oldClaim);
            if (!result.Succeeded)
            {
                var errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));

                return Result<string>.Failure(
                    ResultStatus.Failure,
                    $"Claim removal failed. Details:: {errorMessage}"
                );
            }

            return Result<string>.Success(user.Id);

        }
    }
}
