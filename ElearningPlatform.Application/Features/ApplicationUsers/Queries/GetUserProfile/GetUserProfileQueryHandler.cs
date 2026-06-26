using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Features.ApplicationUsers.Dtos;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.ApplicationUsers.Queries.GetUserProfile
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, Result<ApplicationUserDto>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICurrentUserService currentUserService;

        public GetUserProfileQueryHandler(UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService)
        {
            this.userManager = userManager;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<ApplicationUserDto>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(currentUserService.UserId!);


            if (user == null)
                return Result<ApplicationUserDto>.Failure(
                 ResultStatus.Unauthorized,
                   "The current authenticated user could not be found. Please log in again.");
            var userDto = new ApplicationUserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName!,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender.ToString(),
                
            };

            return Result<ApplicationUserDto>.Success(userDto);
        }
    }
}
