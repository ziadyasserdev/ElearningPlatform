using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Categories.Commands.BulkRestoreCategory
{
    public class BulkRestoreCategoryCommand : IRequest<Result<string>>
    {
        public List<int> CategoryIds { get; set; } = new();
    }
}
