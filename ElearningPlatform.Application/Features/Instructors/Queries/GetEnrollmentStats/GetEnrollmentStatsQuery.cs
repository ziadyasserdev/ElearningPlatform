using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetEnrollmentStats
{
    public class GetEnrollmentStatsQuery
         : IRequest<Result<EnrollmentStatsDto>>
    {
        public int CourseId { get; set; }
    }
    public class EnrollmentStatsDto
    {
        public int TotalStudents { get; set; }
        public int CompletedStudents { get; set; }
        public double CompletionRate { get; set; }
    }
}