using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Authentications.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authentications.Commands.VerifyTwoFactor
{
    public class VerifyTwoFactorCommand : IRequest<Result<AuthTokenDto>>
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
