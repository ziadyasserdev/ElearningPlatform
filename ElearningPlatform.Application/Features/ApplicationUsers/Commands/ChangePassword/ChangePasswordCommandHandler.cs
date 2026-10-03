using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.ApplicationUsers.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler
    : IRequestHandler<ChangePasswordCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICurrentUserService currentUserService;
        private readonly ILogger<ChangePasswordCommandHandler> logger;

        public ChangePasswordCommandHandler(
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService,
            ILogger<ChangePasswordCommandHandler> logger)
        {
            this.userManager = userManager;
            this.currentUserService = currentUserService;
            this.logger = logger;
        }

        public async Task<Result<string>> Handle(
            ChangePasswordCommand request,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Password change attempt started for UserId: {UserId}",
                currentUserService.UserId);

            var currentUser = await userManager.FindByIdAsync(currentUserService.UserId!);

            if (currentUser == null)
            {
                logger.LogWarning(
                    "Password change failed because the current user was not found. UserId: {UserId}",
                    currentUserService.UserId);

                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "The current authenticated user could not be found. Please log in again.");
            }

            if (!await userManager.CheckPasswordAsync(
                    currentUser,
                    request.oldPassword))
            {
                logger.LogWarning(
                    "Password change failed because the current password is incorrect. UserId: {UserId}",
                    currentUser.Id);

                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "The current password is incorrect.");
            }

            var result = await userManager.ChangePasswordAsync(
                currentUser,
                request.oldPassword,
                request.newPassword);

            if (!result.Succeeded)
            {
                var errorMessage = string.Join(
                    " | ",
                    result.Errors.Select(e => e.Description));

                logger.LogError(
                    "Password change failed for UserId: {UserId}. Errors: {Errors}",
                    currentUser.Id,
                    errorMessage);

                return Result<string>.Failure(
                    ResultStatus.Failure,
                    $"Password change failed. Details: {errorMessage}");
            }

            logger.LogInformation(
                "Password changed successfully for UserId: {UserId}",
                currentUser.Id);

            return Result<string>.Success(currentUser.Id);
        }
    }
  
}
