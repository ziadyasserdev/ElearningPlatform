using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.ApplicationUsers.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICurrentUserService currentUserService;

        public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService)
        {
            this.userManager = userManager;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await userManager.FindByIdAsync(currentUserService.UserId!);
            if (currentUser == null)
                return Result<string>.Failure(
        ResultStatus.Unauthorized,
        "The current authenticated user could not be found. Please log in again.");
            if (!await userManager.CheckPasswordAsync(currentUser, request.oldPassword))
                return Result<string>.Failure(
        ResultStatus.Failure,
        "The current password is incorrect.");
            var result = await userManager.ChangePasswordAsync(currentUser, request.oldPassword, request.newPassword);

            if (!result.Succeeded)
            {
                var errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));

                return Result<string>.Failure(
                    ResultStatus.Failure,
                    $"Password change failed. Details: {errorMessage}"
                );
            }

            return Result<string>.Success(currentUser.Id);
        }

    }
}
