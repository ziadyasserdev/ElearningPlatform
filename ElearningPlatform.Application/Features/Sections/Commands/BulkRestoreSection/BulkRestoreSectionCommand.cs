using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Sections.Commands.BulkRestoreSection
{
    public class BulkRestoreSectionCommand:IRequest<Result<string>>
    {
        public List<int> SectionIds { get; set; } = new();
    }
}
