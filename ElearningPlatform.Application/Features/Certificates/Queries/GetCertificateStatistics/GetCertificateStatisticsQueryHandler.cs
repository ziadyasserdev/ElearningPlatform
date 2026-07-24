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

namespace ElearningPlatform.Application.Features.Certificates.Queries.GetCertificateStatistics
{
    public class GetCertificateStatisticsQueryHandler
      : IRequestHandler<GetCertificateStatisticsQuery, Result<CertificateStatisticsDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetCertificateStatisticsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<CertificateStatisticsDto>> Handle(
            GetCertificateStatisticsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<CertificateStatisticsDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required.");
            }

            if (!currentUserService.IsInRole("Admin"))
            {
                return Result<CertificateStatisticsDto>.Failure(
                    ResultStatus.Forbidden,
                    "Only administrators can access certificate statistics.");
            }

            var totalCertificates = await unitOfWork.Certificates.Query()
                .CountAsync(x => !x.IsDeleted, cancellationToken);

            var activeCertificates = await unitOfWork.Certificates.Query()
                .CountAsync(x => !x.IsDeleted && !x.IsRevoked, cancellationToken);

            var revokedCertificates = await unitOfWork.Certificates.Query()
                .CountAsync(x => !x.IsDeleted && x.IsRevoked, cancellationToken);

            var totalDownloads = await unitOfWork.Certificates.Query()
                .Where(x => !x.IsDeleted)
                .SumAsync(x => x.DownloadCount, cancellationToken);

            var now = DateTime.UtcNow;

            var generatedThisMonth = await unitOfWork.Certificates.Query()
                .CountAsync(x =>
                    !x.IsDeleted &&
                    x.IssuedAt.Year == now.Year &&
                    x.IssuedAt.Month == now.Month,
                    cancellationToken);

            return Result<CertificateStatisticsDto>.Success(
                new CertificateStatisticsDto
                {
                    TotalCertificates = totalCertificates,
                    ActiveCertificates = activeCertificates,
                    RevokedCertificates = revokedCertificates,
                    TotalDownloads = totalDownloads,
                    GeneratedThisMonth = generatedThisMonth
                });
        }
    }
}
