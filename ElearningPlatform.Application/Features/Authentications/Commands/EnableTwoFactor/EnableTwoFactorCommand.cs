using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authentications.Commands.EnableTwoFactor
{
    public class EnableTwoFactorCommand : IRequest<Result<string>>
    {
        public string UserId { get; set; }
    }
}
