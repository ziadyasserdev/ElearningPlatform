using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authentications.Commands.LockUser
{
    public class LockUserCommand : IRequest<Result<string>>
    {
        public string UserId { get; set; }

        public LockUserCommand(string userId)
        {
            UserId = userId;
        }
    }
}
