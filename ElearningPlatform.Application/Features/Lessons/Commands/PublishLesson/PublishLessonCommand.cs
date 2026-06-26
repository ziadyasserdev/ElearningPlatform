using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Lessons.Commands.PublishLesson
{
    public class PublishLessonCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }
}
