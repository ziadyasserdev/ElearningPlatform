using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Commands.UpdateRoleOfUser
{
    public class UpdateRoleOfUserCommand : IRequest<Result<string>>
    {
        public string UserId { get; set; }
        public string oldRoleId { get; set; }
        public string NewRoleId { get; set; }
    }
}
