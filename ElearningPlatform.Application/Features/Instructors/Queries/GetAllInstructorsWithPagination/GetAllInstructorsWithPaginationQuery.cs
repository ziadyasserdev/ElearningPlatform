using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Instructors.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetAllInstructorsWithPagination
{
    public class GetAllInstructorsWithPaginationQuery:IRequest<Result<PaginatedResult<InstructorDto>>>
    {
            public int  PageSize { get; set; }
            public int PageNumber { get; set; }
        public GetAllInstructorsWithPaginationQuery(int ps,int pn)
        {
            PageSize = ps;
            PageNumber = pn;
        }
    }
}
