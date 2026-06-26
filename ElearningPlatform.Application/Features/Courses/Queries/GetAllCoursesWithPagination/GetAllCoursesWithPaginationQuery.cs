using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Queries.GetAllCoursesWithPagination
{
    public class GetAllCoursesWithPaginationQuery:IRequest<Result<PaginatedResult<object>>>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public GetAllCoursesWithPaginationQuery(int pn,int ps)
        {
            PageSize = ps;
            PageNumber = pn;
        }
    }
}
