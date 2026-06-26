using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.ApplicationUsers.Dtos;
using ElearningPlatform.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.ApplicationUsers.Queries.GetAllUsersByStatus
{
    public class GetAllUsersByStatusQuery : IRequest<Result<PaginatedResult<ApplicationUserDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public UserStatus UserStatus { get; set; }
        public GetAllUsersByStatusQuery(int pn, int ps, UserStatus userStatus)
        {
            PageNumber = pn;
            PageSize = ps;
            UserStatus = userStatus;
        }
    }
}
