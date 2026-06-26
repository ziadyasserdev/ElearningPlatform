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

namespace ElearningPlatform.Application.Features.ApplicationUsers.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<ApplicationUserDto>>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public GetUserByIdQueryHandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Result<ApplicationUserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.userId);
            if (user == null)
                return Result<ApplicationUserDto>.Failure(ResultStatus.NotFound, "User not found.");
            var userDto = new ApplicationUserDto
            {
                Id = request.userId,
                UserName = user.UserName!,
                Email = user.Email!,
                FullName = user.FullName!,
                PhoneNumber = user.PhoneNumber,
               
                Gender = user.Gender.ToString()

            };
            return Result<ApplicationUserDto>.Success(userDto);
        }
    }
}
