using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Lessons.Commands.ReorderLessons
{
    public class ReorderLessonsCommand : IRequest<Result<string>>
    {
        public int SectionId { get; set; }
        public List<int> LessonIds { get; set; }
    }
}
