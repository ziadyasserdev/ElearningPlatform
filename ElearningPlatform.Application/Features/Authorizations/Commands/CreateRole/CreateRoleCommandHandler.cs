using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Authorizations.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Commands.CreateRole
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Result<RoleDto>>
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public CreateRoleCommandHandler(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        public async Task<Result<RoleDto>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var checkExis = await roleManager.RoleExistsAsync(request.roleName);
            if(checkExis) 
                return Result<RoleDto>.Failure(ResultStatus.Conflict,"Role already exists");
            var role = new IdentityRole
            {
                Name = request.roleName
            };
            var result = await roleManager.CreateAsync(role);
            if(!result.Succeeded)
            {
               var error = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<RoleDto>.Failure(ResultStatus.Failure,error);
            }
            return Result<RoleDto>.Success(new RoleDto { Id =role.Id , Name = request.roleName }, "Role created successfully");
        }
    }
}
