using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Sections.Commands.ReorderSection
{
    public class ReorderSectionCommand : IRequest<Result<int>>
    {
        public int SectionId { get; set; }
        public int NewOrder { get; set; }
    }
}
