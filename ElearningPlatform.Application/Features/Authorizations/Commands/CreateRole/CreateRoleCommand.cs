using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Authorizations.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Commands.CreateRole
{
    public class CreateRoleCommand:IRequest<Result<RoleDto>>
    {
        public string roleName { get; set; }
        public CreateRoleCommand(string roleName)
        {
            this.roleName = roleName;
        }
    }
}
