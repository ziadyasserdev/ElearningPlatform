using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Services;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.ApplicationUsers.Commands.DeleteImageProfile
{
    public class DeleteImageProfileCommandHandler : IRequestHandler<DeleteImageProfileCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFileService fileService;
        private readonly ICurrentUserService currentUserService;

        public DeleteImageProfileCommandHandler(UserManager<ApplicationUser> userManager, IFileService fileService, ICurrentUserService currentUserService)
        {
            this.userManager = userManager;
            this.fileService = fileService;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(DeleteImageProfileCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "User not authenticated.");

            var userId = currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
                return Result<string>.Failure(ResultStatus.Unauthorized, "User ID not found.");

            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
                return Result<string>.Failure(ResultStatus.NotFound, "User not found.");
            if (user.ProfileImageUrl is null)
                return Result<string>.Failure(ResultStatus.Conflict, "You don't have personal image");
            var removeImage = fileService.Remove(user.ProfileImageUrl);
            if (removeImage is null)
                return Result<string>.Failure(ResultStatus.Failure, removeImage!.Error);

            user.ProfileImageUrl = null;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));

                return Result<string>.Failure(
              ResultStatus.Failure,
              "Failed to add user's ProfileImage. Please try again later."
          );


            }

            return Result<string>.Success(user.Id);
        }
    }
}
