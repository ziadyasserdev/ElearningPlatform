using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Queries.GetEnrollmentAnalytics
{
    public class GetEnrollmentAnalyticsQueryValidator
       : AbstractValidator<GetEnrollmentAnalyticsQuery>
    {
        public GetEnrollmentAnalyticsQueryValidator()
        {
            RuleFor(x => x.Year)
                .InclusiveBetween(2000, DateTime.UtcNow.Year)
                .WithMessage($"Year must be between 2000 and {DateTime.UtcNow.Year}.");
        }
    }
}
