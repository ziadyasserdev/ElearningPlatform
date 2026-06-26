using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Lessons.Commands.ToggleLessonPreview
{
    public class ToggleLessonPreviewCommand : IRequest<Result<int>>
    {
        public int LessonId { get; set; }
    }
}
