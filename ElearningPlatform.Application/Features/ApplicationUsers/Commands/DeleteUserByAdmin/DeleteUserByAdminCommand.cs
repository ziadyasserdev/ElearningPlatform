using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.ApplicationUsers.Commands.DeleteUserByAdmin
{
    public class DeleteUserByAdminCommand : IRequest<Result<string>>
    {
        public string UserId { get; set; }
        public DeleteUserByAdminCommand(string u)
        {
            UserId = u;
        }

    }
}
