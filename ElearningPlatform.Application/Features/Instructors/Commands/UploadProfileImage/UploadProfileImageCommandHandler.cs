using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Contracts.Services;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Commands.UploadProfileImage
{
    public class UploadProfileImageCommandHandler : IRequestHandler<UploadProfileImageCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFileService fileService;
        private readonly ICurrentUserService currentUserService;

        public UploadProfileImageCommandHandler(UserManager<ApplicationUser> userManager ,IFileService fileService,ICurrentUserService currentUserService)
        {
            this.userManager = userManager;
            this.fileService = fileService;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(UploadProfileImageCommand request, CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "User must be authenticated to upload profile image");
            var userId = currentUserService.UserId;
            if(userId is null)
                return Result<string>.Failure(ResultStatus.Unauthorized, "User must be authenticated to upload profile image");
            var user = await userManager.FindByIdAsync(userId);
            if(user is null)
                return Result<string>.Failure(ResultStatus.NotFound, "User not found");
            var uploadResult = await fileService.UploadImageAsync(request.Image);

            if (!uploadResult.IsSuccess)
                return Result<string>.Failure(uploadResult.Status, uploadResult.Error);

            if (!string.IsNullOrEmpty(user.ProfileImageUrl))
            {
                fileService.Remove(user.ProfileImageUrl);
            }

            user.ProfileImageUrl = uploadResult.Value!;
            await userManager.UpdateAsync(user);

            return Result<string>.Success("Image Uploaded Successfully");

        }
    }
}
