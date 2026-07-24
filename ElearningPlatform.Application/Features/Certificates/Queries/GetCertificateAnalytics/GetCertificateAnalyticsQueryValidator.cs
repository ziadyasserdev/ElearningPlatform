using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Certificates.Queries.GetCertificateAnalytics
{
    public class GetCertificateAnalyticsQueryValidator
       : AbstractValidator<GetCertificateAnalyticsQuery>
    {
        public GetCertificateAnalyticsQueryValidator()
        {
            RuleFor(x => x.Year)
                .InclusiveBetween(2000, DateTime.UtcNow.Year);
        }
    }
}
