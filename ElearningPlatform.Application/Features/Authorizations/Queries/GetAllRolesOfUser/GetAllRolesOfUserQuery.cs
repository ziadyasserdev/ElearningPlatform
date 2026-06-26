using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Authorizations.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authorizations.Queries.GetAllRolesOfUser
{
    public class GetAllRolesOfUserQuery : IRequest<Result<List<UserRoleDto>>>
    {
        public string UserId { get; set; }
        public GetAllRolesOfUserQuery(string u)
        {
            UserId = u;
        }
    }
}
