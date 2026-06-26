using ElearningPlatform.Application.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Commands.UpdateCourseThumbnail
{
    public class UpdateCourseThumbnailCommand : IRequest<Result<string>>
    {
        public int CourseId { get; set; }
        public IFormFile File { get; set; }
    }
}
