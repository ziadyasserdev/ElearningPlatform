using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Courses.Dtos;
using ElearningPlatform.Application.Features.Courses.Queries.GetFeaturedCourses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Queries.GetLatestCourses
{
    public class GetLatestCoursesQuery
    : IRequest<Result<List<CourseUserDtoo>>>
    {
    }
}
