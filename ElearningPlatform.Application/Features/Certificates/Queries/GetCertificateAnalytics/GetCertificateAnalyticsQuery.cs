using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Certificates.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Certificates.Queries.GetCertificateAnalytics
{

    public class GetCertificateAnalyticsQuery
        : IRequest<Result<List<CertificateAnalyticsDto>>>
    {
        public int Year { get; set; } = DateTime.UtcNow.Year;
    }
}
