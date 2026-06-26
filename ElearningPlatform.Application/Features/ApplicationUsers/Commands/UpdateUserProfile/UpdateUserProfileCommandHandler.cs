using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.ApplicationUsers.Commands.UpdateUserProfile
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICurrentUserService currentUserService;

        public UpdateUserProfileCommandHandler(UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService)
        {
            this.userManager = userManager;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(currentUserService.UserId))
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");

            var user = await userManager.FindByIdAsync(currentUserService.UserId!);
            if (user == null)
                return Result<string>.Failure(
                 ResultStatus.Unauthorized,
                   "The current authenticated user could not be found. Please log in again.");


            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                var normalizedEmail = userManager.NormalizeEmail(request.Email);

                var emailExist = await userManager.Users
                    .AnyAsync(x => x.NormalizedEmail == normalizedEmail && x.Id != user.Id);
                if (emailExist)
                {
                    return Result<string>.Failure(
                        ResultStatus.Conflict,
                        "This email address is already in use by another account."
                    );
                }
                var setEmailResult = await userManager.SetEmailAsync(user, request.Email);
                if (!setEmailResult.Succeeded)
                {
                    var errorMessage = string.Join(" | ", setEmailResult.Errors.Select(e => e.Description));

                    return Result<string>.Failure(
                        ResultStatus.Failure,
                        $"Update profile details failed. Details: {errorMessage}"
                    );
                }
                user.EmailConfirmed = false;
            }

            if (!string.IsNullOrWhiteSpace(request.UserName))
            {
                var normalizeUserName = userManager.NormalizeName(request.UserName);
                var userNameExist = await userManager.Users
                    .AnyAsync(x => x.NormalizedUserName == normalizeUserName && x.Id != user.Id);

                if (userNameExist)
                {
                    return Result<string>.Failure(
                        ResultStatus.Conflict,
                        "This username is already in use by another account."
                    );
                }
                var SetUserName = await userManager.SetUserNameAsync(user, request.UserName);
                if (!SetUserName.Succeeded)
                {
                    var errorMessage = string.Join(" | ", SetUserName.Errors.Select(e => e.Description));

                    return Result<string>.Failure(
                        ResultStatus.Failure,
                        $"Update profile details failed. Details: {errorMessage}"
                    );
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
                var errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));

                return Result<string>.Failure(
                    ResultStatus.Failure,
                    $"Update profile details failed. Details: {errorMessage}"
                );
            }
            return Result<string>.Success(user.Id);
        }
    }
}
