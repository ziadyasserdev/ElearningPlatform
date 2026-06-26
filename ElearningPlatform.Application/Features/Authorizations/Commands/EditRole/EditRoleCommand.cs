using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Authorizations.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Commands.EditRole
{
    public class EditRoleCommand : IRequest<Result<RoleDto>>
    {
        public string roleId { get; set; }
        public string newRoleName { get; set; }

    }
}
