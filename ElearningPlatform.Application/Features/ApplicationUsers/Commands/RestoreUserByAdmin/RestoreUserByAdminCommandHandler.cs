using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.ApplicationUsers.Commands.RestoreUserByAdmin
{
    public class RestoreUserByAdminCommandHandler : IRequestHandler<RestoreUserByAdminCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public RestoreUserByAdminCommandHandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Result<string>> Handle(RestoreUserByAdminCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.UserId);

            if (user is null)
                return Result<string>.Failure(ResultStatus.NotFound, $"User with id {request.UserId} not found");

            if (!user.IsDeleted)
                return Result<string>.Failure(ResultStatus.Conflict, $"User with id {request.UserId} already active");

            user.IsDeleted = false;
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));

                return Result<string>.Failure(
                    ResultStatus.Failure,
                    $"Restore User failed. Details: {errorMessage}"
                );
            }

            return Result<string>.Success(user.Id);
        }
    }
}
