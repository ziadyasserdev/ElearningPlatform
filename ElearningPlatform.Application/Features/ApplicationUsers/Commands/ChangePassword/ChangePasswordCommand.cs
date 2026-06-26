using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.ApplicationUsers.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest<Result<string>>
    {
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
        public string confirmedPassword { get; set; }

    }
}
