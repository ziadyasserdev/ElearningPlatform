using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Enrollments.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Queries.GetRecentCourses
{
    public class GetRecentCoursesQuery
         : IRequest<Result<List<RecentCourseDto>>>
    {
    }
}
