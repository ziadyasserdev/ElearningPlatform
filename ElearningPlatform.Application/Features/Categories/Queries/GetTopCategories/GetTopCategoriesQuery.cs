using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Categories.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Queries.GetTopCategories
{
    public class GetTopCategoriesQuery : IRequest<Result<List<TopCategoryDto>>>
    {
        public int Limit { get; set; } = 5;
    }
}
