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

namespace ElearningPlatform.Application.Features.Instructors.Commands.DeleteProfileImage
{
    public class DeleteProfileImageCommandHandler : IRequestHandler<DeleteProfileImageCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileService _fileService;

        public DeleteProfileImageCommandHandler(UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService,IFileService fileService)
        {
            this._userManager = userManager;
            this._currentUserService = currentUserService;
            this._fileService = fileService;
        }
        public async Task<Result<string>> Handle(DeleteProfileImageCommand request, CancellationToken cancellationToken)
        {
            if (!_currentUserService.IsAuthenticated || _currentUserService.UserId is null)
                return Result<string>.Failure(ResultStatus.Unauthorized, "User must be authenticated to delete profile image.");

            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);
            if (user is null)
                return Result<string>.Failure(ResultStatus.NotFound, "User not found.");

            if (string.IsNullOrEmpty(user.ProfileImageUrl))
                return Result<string>.Failure(ResultStatus.Failure, "User has no existing profile image to delete.");

         
            var removeResult = _fileService.Remove(user.ProfileImageUrl);
            if (!removeResult.IsSuccess)
            {
              
                return Result<string>.Failure(removeResult.Status, $"Failed to delete profile image: {removeResult.Error}");
            }

            
            user.ProfileImageUrl = null!;
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return Result<string>.Failure(ResultStatus.Failure, "Failed to update user data after deleting profile image.");
            }

            return Result<string>.Success("Profile image deleted successfully.");

        }
    }
}
