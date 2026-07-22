using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Enrollments.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Queries.GetEnrollmentAnalytics
{
    public class GetEnrollmentAnalyticsQuery
         : IRequest<Result<List<EnrollmentAnalyticsDto>>>
    {
        public int Year { get; set; } = DateTime.UtcNow.Year;
    }
}
