using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Certificates.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Certificates.Queries.GetCertificateAnalytics
{
    public class GetCertificateAnalyticsQueryHandler
       : IRequestHandler<GetCertificateAnalyticsQuery, Result<List<CertificateAnalyticsDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetCertificateAnalyticsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<CertificateAnalyticsDto>>> Handle(
            GetCertificateAnalyticsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<List<CertificateAnalyticsDto>>
                    .Failure(
                        ResultStatus.Unauthorized,
                        "Authentication required.");
            }

            if (!currentUserService.IsInRole("Admin"))
            {
                return Result<List<CertificateAnalyticsDto>>
                    .Failure(
                        ResultStatus.Forbidden,
                        "Only administrators can access analytics.");
            }

            var analytics = await unitOfWork.Certificates.Query()
                .Where(x =>
                    !x.IsDeleted &&
                    x.IssuedAt.Year == request.Year)
                .GroupBy(x => x.IssuedAt.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    GeneratedCertificates = g.Count(),
                    Downloads = g.Sum(x => x.DownloadCount)
                })
                .ToListAsync(cancellationToken);

            var result = Enumerable.Range(1, 12)
                .Select(month =>
                {
                    var data = analytics.FirstOrDefault(x => x.Month == month);

                    return new CertificateAnalyticsDto
                    {
                        Month = CultureInfo.InvariantCulture
                            .DateTimeFormat
                            .GetMonthName(month),

                        GeneratedCertificates =
                            data?.GeneratedCertificates ?? 0,

                        Downloads =
                            data?.Downloads ?? 0
                    };
                })
                .ToList();

            return Result<List<CertificateAnalyticsDto>>
                .Success(result);
        }
    }
}
