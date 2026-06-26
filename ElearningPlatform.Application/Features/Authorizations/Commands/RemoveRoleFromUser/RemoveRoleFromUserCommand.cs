using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Commands.RemoveRoleFromUser
{
    public class RemoveRoleFromUserCommand : IRequest<Result<string>>
    {
        public string userId { get; set; }
        public string roleId { get; set; }
    }
}
