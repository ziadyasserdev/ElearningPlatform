using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Commands.RemoveCategoryIcon
{
    public class RemoveCategoryIconCommand:IRequest<Result<string>>
    {
        public int CategoryId { get; set; }
        public RemoveCategoryIconCommand(int id)
        {
            CategoryId = id;
        }
    }
}
