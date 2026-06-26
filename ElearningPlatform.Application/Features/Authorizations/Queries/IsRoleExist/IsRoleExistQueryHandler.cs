using ElearningPlatform.Application.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Queries.IsRoleExist
{
    public class IsRoleExistQueryHandler : IRequestHandler<IsRoleExistQuery, Result<string>>
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public IsRoleExistQueryHandler(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        public async Task<Result<string>> Handle(IsRoleExistQuery request, CancellationToken cancellationToken)
        {
            var role = await roleManager.FindByIdAsync(request.RoleId);
            if (role == null)
                return Result<string>.Failure(ResultStatus.NotFound, $"Role with ID '{request.RoleId}' was not found.");

            return Result<string>.Success(role.Id, $"Role '{role.Name}' exists.");
        }
    }
}
