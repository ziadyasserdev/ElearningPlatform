using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Lessons.Commands.BulkToggleLessonPreview
{
    public class BulkToggleLessonPreviewCommand : IRequest<Result<string>>
    {
        public List<int> LessonIds { get; set; }
        public bool IsPreview { get; set; }
    }
}
