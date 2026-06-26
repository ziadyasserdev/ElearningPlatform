using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.ApplicationUsers.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.ApplicationUsers.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<Result<ApplicationUserDto>>
    {
        public string userId { get; set; }
    }
}
