using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Commands.DeleteCourse
{
    public class DeleteCourseCommand : IRequest<Result<int>>
    {
        public int CourseId { get; set; }

        public DeleteCourseCommand(int courseId)
        {
            CourseId = courseId;
        }
    }
}
