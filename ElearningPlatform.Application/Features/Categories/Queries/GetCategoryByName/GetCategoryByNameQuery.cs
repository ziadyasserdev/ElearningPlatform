using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Categories.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Queries.GetCategoryByName
{
    public class GetCategoryByNameQuery:IRequest<Result<CategoryDto>>
    {
        public string Name { get; set; }
        public GetCategoryByNameQuery(string n)
        {
            Name = n;
        }
    }
}
