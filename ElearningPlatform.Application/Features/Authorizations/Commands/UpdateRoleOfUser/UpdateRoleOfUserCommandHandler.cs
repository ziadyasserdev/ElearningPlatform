using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Commands.UpdateRoleOfUser
{
    public class UpdateRoleOfUserCommandHandler : IRequestHandler<UpdateRoleOfUserCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UpdateRoleOfUserCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<Result<string>> Handle(UpdateRoleOfUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    $"User with ID '{request.UserId}' was not found."
                );

            var oldRole = await roleManager.FindByIdAsync(request.oldRoleId);
            if (oldRole == null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Old role not found."
                );
            if (!await userManager.IsInRoleAsync(user, oldRole.Name!))
                return Result<string>.Failure(ResultStatus.Failure, "User is not assigned to the specified old role.");
            var newRole = await roleManager.FindByIdAsync(request.NewRoleId);
            if (newRole == null)
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "New role not found."
                );

            if (await userManager.IsInRoleAsync(user, newRole.Name!))
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    $"User is already assigned to role '{newRole.Name}'."
                );
            var resultOfRemove = await userManager.RemoveFromRoleAsync(user, oldRole.Name!);
            if (!resultOfRemove.Succeeded)
            {
                var errorMessage = string.Join(" | ", resultOfRemove.Errors.Select(e => e.Description));

                return Result<string>.Failure(
                    ResultStatus.Failure,
                    $"Role assignment failed. Details: {errorMessage}"
                );
            }

            var resultOfAdd = await userManager.AddToRoleAsync(user, newRole.Name!);
            if (!resultOfAdd.Succeeded)
            {
                var errorMessage = string.Join(" | ", resultOfAdd.Errors.Select(e => e.Description));

                return Result<string>.Failure(
                    ResultStatus.Failure,
                    $"Role assignment failed. Details: {errorMessage}"
                );
            }
            return Result<string>.Success(
              user.Id,
              $"Role '{newRole.Name}' assigned to user successfully."
          );
        }
    }
}
