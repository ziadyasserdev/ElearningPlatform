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

namespace ElearningPlatform.Application.Features.ApplicationUsers.Commands.DeleteImageProfile
{
    public class DeleteImageProfileCommandHandler
         : IRequestHandler<DeleteImageProfileCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFileService fileService;
        private readonly ICurrentUserService currentUserService;
        private readonly ILogger<DeleteImageProfileCommandHandler> logger;

        public DeleteImageProfileCommandHandler(
            UserManager<ApplicationUser> userManager,
            IFileService fileService,
            ICurrentUserService currentUserService,
            ILogger<DeleteImageProfileCommandHandler> logger)
        {
            this.userManager = userManager;
            this.fileService = fileService;
            this.currentUserService = currentUserService;
            this.logger = logger;
        }

        public async Task<Result<string>> Handle(
            DeleteImageProfileCommand request,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Profile image deletion started for UserId: {UserId}",
                currentUserService.UserId);

            if (!currentUserService.IsAuthenticated)
            {
                logger.LogWarning(
                    "Profile image deletion failed because the user is not authenticated.");

                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "User not authenticated.");
            }

            var userId = currentUserService.UserId;

            if (string.IsNullOrEmpty(userId))
            {
                logger.LogWarning(
                    "Profile image deletion failed because UserId was not found.");

                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "User ID not found.");
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user is null)
            {
                logger.LogWarning(
                    "Profile image deletion failed because the user was not found. UserId: {UserId}",
                    userId);

                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "User not found.");
            }

            if (user.ProfileImageUrl is null)
            {
                logger.LogWarning(
                    "Profile image deletion failed because the user does not have a profile image. UserId: {UserId}",
                    userId);

                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "You don't have personal image");
            }

            var removeImage = fileService.Remove(user.ProfileImageUrl);

            if (removeImage is null)
            {
                logger.LogError(
                    "Profile image removal failed. File service returned null. UserId: {UserId}",
                    userId);

                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Failed to remove profile image.");
            }

            if (!removeImage.IsSuccess)
            {
                logger.LogError(
                    "Profile image removal failed for UserId: {UserId}. Error: {Error}",
                    userId,
                    removeImage.Error);

                return Result<string>.Failure(
                    ResultStatus.Failure,
                    removeImage.Error);
            }

            user.ProfileImageUrl = null;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errorMessage = string.Join(
                    " | ",
                    result.Errors.Select(e => e.Description));

                logger.LogError(
                    "Failed to update user after deleting profile image. UserId: {UserId}. Errors: {Errors}",
                    userId,
                    errorMessage);

                return Result<string>.Failure(
                    ResultStatus.Failure,
                    "Failed to delete user's ProfileImage. Please try again later.");
            }

            logger.LogInformation(
                "Profile image deleted successfully for UserId: {UserId}",
                userId);

            return Result<string>.Success(user.Id);
        }
    }
}
