using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Queries.IsRoleExist
{
    public class IsRoleExistQuery : IRequest<Result<string>>
    {
        public string RoleId { get; set; }
        public IsRoleExistQuery(string RoleId)
        {
            this.RoleId = RoleId;
        }
    }
}

