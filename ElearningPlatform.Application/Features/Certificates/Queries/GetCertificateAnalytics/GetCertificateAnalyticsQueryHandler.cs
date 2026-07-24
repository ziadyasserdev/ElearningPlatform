using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Certificates.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                    .Failure(ResultStatus.Unauthorized,
                        "Authentication required.");
            }

            if (!currentUserService.IsInRole("Admin"))
            {
                return Result<List<CertificateAnalyticsDto>>
                    .Failure(ResultStatus.Forbidden,
                        "Only administrators can access analytics.");
            }

            var data = await unitOfWork.Certificates.Query()
                .Where(x =>
                    !x.IsDeleted &&
                    x.IssuedAt.Year == request.Year)
                .GroupBy(x => x.IssuedAt.Month)
                .Select(g => new CertificateAnalyticsDto
                {
                    Month = new DateTime(request.Year, g.Key, 1)
                        .ToString("MMMM"),

                    GeneratedCertificates = g.Count(),

                    Downloads = g.Sum(x => x.DownloadCount)
                })
                .OrderBy(x => DateTime.ParseExact(
                    x.Month,
                    "MMMM",
                    System.Globalization.CultureInfo.InvariantCulture).Month)
                .ToListAsync(cancellationToken);

            return Result<List<CertificateAnalyticsDto>>
                .Success(data);
        }
    }
}
