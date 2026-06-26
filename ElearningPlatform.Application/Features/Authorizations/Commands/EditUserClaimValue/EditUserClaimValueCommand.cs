using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Commands.EditUserClaimValue
{
    public class EditUserClaimValueCommand : IRequest<Result<string>>
    {
        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string oldClaimValue { get; set; }
        public string NewClaimValue { get; set; }
    }
}
