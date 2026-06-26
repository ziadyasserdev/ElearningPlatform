using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Authorizations.Dtos;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Commands.AddRoleToUser
{
    public class AddRoleToUserCommandHandler : IRequestHandler<AddRoleToUserCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AddRoleToUserCommandHandler(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<Result<string>> Handle(AddRoleToUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.userId);
            if (user == null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    $"User with ID '{request.userId}' was not found."
                );

            var role = await roleManager.FindByIdAsync(request.roleId);
            if (role == null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    $"Role with ID '{request.roleId}' was not found."
                );

            if (await userManager.IsInRoleAsync(user, role.Name!))
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    $"User is already assigned to role '{role.Name}'."
                );

            var result = await userManager.AddToRoleAsync(user, role.Name!);
            if (!result.Succeeded)
            {
                var error = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<string>.Failure(ResultStatus.Failure, error);
            }
            return Result<string>.Success(
              user.Id,
              $"Role '{role.Name}' assigned to user successfully."
          );
        }
    }
}
