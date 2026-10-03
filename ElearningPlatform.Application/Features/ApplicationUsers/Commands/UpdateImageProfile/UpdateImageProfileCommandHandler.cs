using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Services;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.ApplicationUsers.Commands.UpdateImageProfile
{
    public class UpdateImageProfileCommandHandler
        : IRequestHandler<UpdateImageProfileCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFileService fileService;
        private readonly ICurrentUserService currentUserService;
        private readonly ILogger<UpdateImageProfileCommandHandler> logger;

        public UpdateImageProfileCommandHandler(
            UserManager<ApplicationUser> userManager,
            IFileService fileService,
            ICurrentUserService currentUserService,
            ILogger<UpdateImageProfileCommandHandler> logger)
        {
            this.userManager = userManager;
            this.fileService = fileService;
            this.currentUserService = currentUserService;
            this.logger = logger;
        }

        public async Task<Result<string>> Handle(
            UpdateImageProfileCommand request,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Profile image update started for UserId: {UserId}",
                currentUserService.UserId);

            if (!currentUserService.IsAuthenticated)
            {
                logger.LogWarning(
                    "Profile image update failed because the user is not authenticated.");

                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "User not authenticated.");
            }

            var userId = currentUserService.UserId;

            if (string.IsNullOrEmpty(userId))
            {
                logger.LogWarning(
                    "Profile image update failed because UserId was not found.");

                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "User ID not found.");
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user is null)
            {
                logger.LogWarning(
                    "Profile image update failed because the user was not found. UserId: {UserId}",
                    userId);

                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "User not found.");
            }

            if (user.ProfileImageUrl is not null)
            {
                logger.LogInformation(
                    "Removing old profile image for UserId: {UserId}",
                    userId);

                var resRemove = fileService.Remove(user.ProfileImageUrl);

                if (!resRemove.IsSuccess)
                {
                    logger.LogError(
                        "Failed to remove old profile image for UserId: {UserId}. Error: {Error}",
                        userId,
                        resRemove.Error);

                    return Result<string>.Failure(
                        ResultStatus.Failure,
                        resRemove.Error);
                }
            }

            logger.LogInformation(
                "Uploading new profile image for UserId: {UserId}",
                userId);

            var res = await fileService.UploadImageAsync(request.ProfileImage!);

            if (!res.IsSuccess)
            {
                logger.LogError(
                    "Failed to upload new profile image for UserId: {UserId}. Error: {Error}",
                    userId,
                    res.Error);

                return Result<string>.Failure(
                    res.Status,
                    res.Error);
            }

            user.ProfileImageUrl = res.Value!;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errorMessage = string.Join(
                    " | ",
                    result.Errors.Select(e => e.Description));

                logger.LogError(
                    "Failed to update profile image URL in database for UserId: {UserId}. Errors: {Errors}",
                    userId,
                    errorMessage);

                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Failed to update user's ProfileImage. Please try again later.");
            }

            logger.LogInformation(
                "Profile image updated successfully for UserId: {UserId}",
                user.Id);

            return Result<string>.Success(user.Id);
        }
    }
}
