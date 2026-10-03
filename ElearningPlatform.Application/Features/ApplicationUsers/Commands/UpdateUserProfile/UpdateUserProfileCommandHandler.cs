using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.ApplicationUsers.Commands.UpdateUserProfile
{
    public class UpdateUserProfileCommandHandler
       : IRequestHandler<UpdateUserProfileCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICurrentUserService currentUserService;
        private readonly ILogger<UpdateUserProfileCommandHandler> logger;

        public UpdateUserProfileCommandHandler(
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService,
            ILogger<UpdateUserProfileCommandHandler> logger)
        {
            this.userManager = userManager;
            this.currentUserService = currentUserService;
            this.logger = logger;
        }

        public async Task<Result<string>> Handle(
            UpdateUserProfileCommand request,
            CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            logger.LogInformation(
                "Profile update started for UserId: {UserId}",
                userId);

            if (string.IsNullOrEmpty(userId))
            {
                logger.LogWarning(
                    "Profile update failed because the user is not authenticated.");

                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                logger.LogWarning(
                    "Profile update failed because the current user was not found. UserId: {UserId}",
                    userId);

                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "The current authenticated user could not be found. Please log in again.");
            }

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                var normalizedEmail = userManager.NormalizeEmail(request.Email);

                var emailExist = await userManager.Users
                    .AnyAsync(
                        x => x.NormalizedEmail == normalizedEmail && x.Id != user.Id,
                        cancellationToken);

                if (emailExist)
                {
                    logger.LogWarning(
                        "Profile update failed because the email is already in use. UserId: {UserId}",
                        user.Id);

                    return Result<string>.Failure(
                        ResultStatus.Conflict,
                        "This email address is already in use by another account.");
                }

                logger.LogInformation(
                    "Updating email for UserId: {UserId}",
                    user.Id);

                var setEmailResult = await userManager.SetEmailAsync(
                    user,
                    request.Email);

                if (!setEmailResult.Succeeded)
                {
                    var errorMessage = string.Join(
                        " | ",
                        setEmailResult.Errors.Select(e => e.Description));

                    logger.LogError(
                        "Failed to update email for UserId: {UserId}. Errors: {Errors}",
                        user.Id,
                        errorMessage);

                    return Result<string>.Failure(
                        ResultStatus.Failure,
                        $"Update profile details failed. Details: {errorMessage}");
                }

                user.EmailConfirmed = false;
            }

            if (!string.IsNullOrWhiteSpace(request.UserName))
            {
                var normalizedUserName = userManager.NormalizeName(
                    request.UserName);

                var userNameExist = await userManager.Users
                    .AnyAsync(
                        x => x.NormalizedUserName == normalizedUserName && x.Id != user.Id,
                        cancellationToken);

                if (userNameExist)
                {
                    logger.LogWarning(
                        "Profile update failed because the username is already in use. UserId: {UserId}",
                        user.Id);

                    return Result<string>.Failure(
                        ResultStatus.Conflict,
                        "This username is already in use by another account.");
                }

                logger.LogInformation(
                    "Updating username for UserId: {UserId}",
                    user.Id);

                var setUserNameResult = await userManager.SetUserNameAsync(
                    user,
                    request.UserName);

                if (!setUserNameResult.Succeeded)
                {
                    var errorMessage = string.Join(
                        " | ",
                        setUserNameResult.Errors.Select(e => e.Description));

                    logger.LogError(
                        "Failed to update username for UserId: {UserId}. Errors: {Errors}",
                        user.Id,
                        errorMessage);

                    return Result<string>.Failure(
                        ResultStatus.Failure,
                        $"Update profile details failed. Details: {errorMessage}");
                }
            }

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                user.PhoneNumber = request.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(request.FullName))
                user.FullName = request.FullName;

            if (request.Gender.HasValue)
                user.Gender = request.Gender.Value;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errorMessage = string.Join(
                    " | ",
                    result.Errors.Select(e => e.Description));

                logger.LogError(
                    "Failed to update profile for UserId: {UserId}. Errors: {Errors}",
                    user.Id,
                    errorMessage);

                return Result<string>.Failure(
                    ResultStatus.Failure,
                    $"Update profile details failed. Details: {errorMessage}");
            }

            logger.LogInformation(
                "Profile updated successfully for UserId: {UserId}",
                user.Id);

            return Result<string>.Success(user.Id);
        }
    }
}
