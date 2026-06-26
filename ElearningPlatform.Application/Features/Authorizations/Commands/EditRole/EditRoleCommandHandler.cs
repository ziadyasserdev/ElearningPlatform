using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Authorizations.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Commands.EditRole
{
    public class EditRoleCommandHandler : IRequestHandler<EditRoleCommand, Result<RoleDto>>
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public EditRoleCommandHandler(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        public async Task<Result<RoleDto>> Handle(EditRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await roleManager.FindByIdAsync(request.roleId);
            if (role is null)
                return Result<RoleDto>.Failure(ResultStatus.NotFound, "Role not found");
            var existingRole = await roleManager.FindByNameAsync(request.newRoleName);

            if (existingRole != null && existingRole.Id != request.roleId)
            {
                return Result<RoleDto>.Failure(
                    ResultStatus.Failure,
                    "Role name already exists."
                );
            }
            role.Name = request.newRoleName;
            role.NormalizedName = request.newRoleName;
            var result = await roleManager.UpdateAsync(role);
            if(!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);
                return Result<RoleDto>.Failure(ResultStatus.Failure, string.Join(", ", errors));
            }
            return Result<RoleDto>.Success(new RoleDto { Id = role.Id, Name = role.Name }, "Role updated successfully");
        }
    }
}
