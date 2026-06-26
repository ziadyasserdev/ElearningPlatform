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

namespace ElearningPlatform.Application.Features.Instructors.Commands.UpdateProfileImage
{
    public class UpdateProfileImageCommandHandler : IRequestHandler<UpdateProfileImageCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICurrentUserService currentUserService;
        private readonly IFileService fileService;

        public UpdateProfileImageCommandHandler(UserManager<ApplicationUser> userManager,ICurrentUserService currentUserService,IFileService fileService)
        {
            this.userManager = userManager;
            this.currentUserService = currentUserService;
            this.fileService = fileService;
        }
        public async Task<Result<string>> Handle(UpdateProfileImageCommand request, CancellationToken cancellationToken)
        {
           if(!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "User is not authenticated");
            var userId = currentUserService.UserId;
            if (userId is null)
                return Result<string>.Failure(ResultStatus.Unauthorized, "User must be authenticated to upload profile image");
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
                return Result<string>.Failure(ResultStatus.NotFound, "User not found");
            if(user.ProfileImageUrl is not null)
            {
                var removeImage = fileService.Remove(user.ProfileImageUrl);
                if (!removeImage.IsSuccess)
                    return Result<string>.Failure(removeImage.Status, removeImage.Error);
            }
            var uploadResult = await fileService.UploadImageAsync(request.formFile);

            if (!uploadResult.IsSuccess)
                return Result<string>.Failure(uploadResult.Status, uploadResult.Error);

            if (!string.IsNullOrEmpty(user.ProfileImageUrl))
            {
                fileService.Remove(user.ProfileImageUrl);
            }

            user.ProfileImageUrl = uploadResult.Value!;
            await userManager.UpdateAsync(user);

            return Result<string>.Success("Image Updated Successfully");
        }
    }
}
