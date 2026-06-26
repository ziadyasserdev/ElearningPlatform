using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.ApplicationUsers.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.ApplicationUsers.Queries.GetAllUsers
{
    public class GetAllUsersQuery:IRequest<Result<List<UserDto>>>
    {
    }
}
