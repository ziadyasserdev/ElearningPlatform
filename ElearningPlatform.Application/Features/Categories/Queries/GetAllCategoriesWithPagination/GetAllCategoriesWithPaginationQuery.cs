using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Queries.GetAllCategoriesWithPagination
{
    public class GetAllCategoriesWithPaginationQuery:IRequest<Result<PaginatedResult<object>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public GetAllCategoriesWithPaginationQuery(int pn,int ps)
        {
            PageNumber = pn;
            PageSize = ps;
        }
    }
}
