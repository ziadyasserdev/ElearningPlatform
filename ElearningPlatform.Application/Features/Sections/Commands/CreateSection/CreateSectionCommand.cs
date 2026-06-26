using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Sections.Commands.CreateSection
{
    public class CreateSectionCommand:IRequest<Result<int>>
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
