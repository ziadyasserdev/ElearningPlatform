using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Courses.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Queries.GetCourseDetails
{
    public class GetCourseDetailsQuery:IRequest<Result<CourseDetailsDto>>
    {
        public int Id { get; set; }
    }
}
