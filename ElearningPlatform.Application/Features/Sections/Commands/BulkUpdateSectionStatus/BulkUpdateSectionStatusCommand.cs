using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Sections.Commands.BulkUpdateSectionStatus
{
    public class BulkUpdateSectionStatusCommand:IRequest<Result<string>>
    {
        public List<int> SectionIds { get; set; } = new();
        public bool IsActive { get; set; }
    }
}
