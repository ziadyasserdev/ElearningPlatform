using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authentications.Commands.UnlockUser
{
    public class UnlockUserCommand : IRequest<Result<string>>
    {
        public string UserId { get; set; }
        public UnlockUserCommand(string userId)
        {
            UserId = userId;
        }
    }
}
