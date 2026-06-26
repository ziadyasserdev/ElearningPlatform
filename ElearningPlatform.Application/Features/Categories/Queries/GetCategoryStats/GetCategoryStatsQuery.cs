using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Categories.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Queries.GetCategoryStats
{
    public class GetCategoryStatsQuery:IRequest<Result<CategoryStatsDto>>
    {
        public int CategoryId { get; set; }
        public GetCategoryStatsQuery(int categoryId)
        {
            CategoryId = categoryId;
        }
    }
}
