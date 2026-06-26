using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand:IRequest<Result<int>>    
    {
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
