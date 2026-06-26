using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Queries.CheckCategoryNameAvailability
{
    public class CheckCategoryNameAvailabilityQuery : IRequest<Result<bool>>
    {
        public string Name { get; set; }

        public CheckCategoryNameAvailabilityQuery(string name)
        {
            Name = name;
        }
    }
}
