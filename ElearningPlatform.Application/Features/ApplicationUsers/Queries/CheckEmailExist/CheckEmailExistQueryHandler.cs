using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.ApplicationUsers.Dtos;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.ApplicationUsers.Queries.CheckEmailExist
{

    public class CheckEmailExistQueryHandler : IRequestHandler<CheckEmailExistQuery, Result<ApplicationUserDto>>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public CheckEmailExistQueryHandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Result<ApplicationUserDto>> Handle(CheckEmailExistQuery request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return Result<ApplicationUserDto>.Failure(ResultStatus.NotFound, $"User with email {request.Email} not found");
            var userRoles = await userManager.GetRolesAsync(user);
            var userDto = new ApplicationUserDto
            {
                Id = user.Id,
                UserName = user.UserName!,
                FullName = user.FullName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender.ToString(),
              
            };
            return Result<ApplicationUserDto>.Success(userDto);
        }
    }
}
