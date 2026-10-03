using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.ApplicationUsers.Commands.RestoreUserByAdmin
{
    public class RestoreUserByAdminCommandHandler
       : IRequestHandler<RestoreUserByAdminCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ILogger<RestoreUserByAdminCommandHandler> logger;

        public RestoreUserByAdminCommandHandler(
            UserManager<ApplicationUser> userManager,
            ILogger<RestoreUserByAdminCommandHandler> logger)
        {
            this.userManager = userManager;
            this.logger = logger;
        }

        public async Task<Result<string>> Handle(
            RestoreUserByAdminCommand request,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Admin started restoring user. UserId: {UserId}",
                request.UserId);

            var user = await userManager.FindByIdAsync(request.UserId);

            if (user is null)
            {
                logger.LogWarning(
                    "User restoration failed because user was not found. UserId: {UserId}",
                    request.UserId);

                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    $"User with id {request.UserId} not found");
            }

            if (!user.IsDeleted)
            {
                logger.LogWarning(
                    "User restoration failed because user is already active. UserId: {UserId}",
                    request.UserId);

                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    $"User with id {request.UserId} already active");
            }

            user.IsDeleted = false;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errorMessage = string.Join(
                    " | ",
                    result.Errors.Select(e => e.Description));

                logger.LogError(
                    "User restoration failed for UserId: {UserId}. Errors: {Errors}",
                    user.Id,
                    errorMessage);

                return Result<string>.Failure(
                    ResultStatus.Failure,
                    $"Restore User failed. Details: {errorMessage}");
            }

            logger.LogInformation(
                "Admin successfully restored user. UserId: {UserId}",
                user.Id);

            return Result<string>.Success(user.Id);
        }
    }
}
