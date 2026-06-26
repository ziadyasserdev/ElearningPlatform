using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authentications.Commands.ResendConfirmation
{
    public class ResendConfirmationCommand : IRequest<Result<string>>
    {
        public string Email { get; set; }
        public ResendConfirmationCommand(string email)
        {
            Email = email;
        }
    }
}
