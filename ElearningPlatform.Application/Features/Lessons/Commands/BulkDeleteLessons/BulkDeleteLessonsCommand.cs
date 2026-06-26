using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Lessons.Commands.BulkDeleteLessons
{
    public class BulkDeleteLessonsCommand : IRequest<Result<string>>
    {
        public List<int> LessonIds { get; set; }
    }
}
