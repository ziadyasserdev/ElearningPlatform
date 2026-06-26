using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Commands.DeleteRole
{
    public class DeleteRoleCommand : IRequest<Result<string>>
    {
        public string RoleId { get; set; }
        public DeleteRoleCommand(string RoleId)
        {
            this.RoleId = RoleId;
        }
    }
}
