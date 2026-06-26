using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authentications.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<Result<string>>
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
