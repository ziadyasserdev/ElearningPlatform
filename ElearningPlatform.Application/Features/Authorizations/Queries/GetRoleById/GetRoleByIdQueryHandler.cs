using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Authorizations.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Queries.GetRoleById
{
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, Result<RoleDto>>
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public GetRoleByIdQueryHandler(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        public async Task<Result<RoleDto>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await roleManager.FindByIdAsync(request.roleId);
            if (role == null)
                return Result<RoleDto>.Failure(ResultStatus.NotFound, "Role not found");
            var roleDto = new RoleDto
            {
                Id = role.Id,
                Name = role.Name!
            };
            return Result<RoleDto>.Success(roleDto);
        }
    }
}
