using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Authorizations.Dtos;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Queries.GetAllRolesOfUser
{
    public class GetAllRolesOfUserQueryHandler:IRequestHandler<GetAllRolesOfUserQuery, Result<List<UserRoleDto>>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        public GetAllRolesOfUserQueryHandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Result<List<UserRoleDto>>> Handle(GetAllRolesOfUserQuery request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return Result<List<UserRoleDto>>.Failure(
                    ResultStatus.NotFound,
                    $"User with ID '{request.UserId}' was not found."
                );
            var roles = await userManager.GetRolesAsync(user);
            var userRolesDto = roles.Select(role => new UserRoleDto
            {
                RoleName = role
            }).ToList();
            return Result<List<UserRoleDto>>.Success(userRolesDto);
        }
    }


}
