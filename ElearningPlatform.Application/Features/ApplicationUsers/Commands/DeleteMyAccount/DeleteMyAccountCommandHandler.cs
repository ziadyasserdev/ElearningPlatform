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

namespace ElearningPlatform.Application.Features.ApplicationUsers.Commands.DeleteMyAccount
{
    public class DeleteMyAccountCommandHandler
       : IRequestHandler<DeleteMyAccountCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICurrentUserService currentUserService;
        private readonly ILogger<DeleteMyAccountCommandHandler> logger;

        public DeleteMyAccountCommandHandler(
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService,
            ILogger<DeleteMyAccountCommandHandler> logger)
        {
            this.userManager = userManager;
            this.currentUserService = currentUserService;
            this.logger = logger;
        }

        public async Task<Result<string>> Handle(
            DeleteMyAccountCommand request,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Account deletion started for UserId: {UserId}",
                currentUserService.UserId);

            var currentUser = await userManager.FindByIdAsync(
                currentUserService.UserId!);

            if (currentUser == null)
            {
                logger.LogWarning(
                    "Account deletion failed because the current user was not found. UserId: {UserId}",
                    currentUserService.UserId);

                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "The current authenticated user could not be found. Please log in again.");
            }

            currentUser.IsDeleted = true;

            var result = await userManager.UpdateAsync(currentUser);

            if (!result.Succeeded)
            {
                var errorMessage = string.Join(
                    " | ",
                    result.Errors.Select(e => e.Description));

                logger.LogError(
                    "Account deletion failed for UserId: {UserId}. Errors: {Errors}",
                    currentUser.Id,
                    errorMessage);

                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Failed to delete your account. Please try again later.");
            }

            logger.LogInformation(
                "Account deleted successfully for UserId: {UserId}",
                currentUser.Id);

            return Result<string>.Success(currentUser.Id);
        }
    }
}
