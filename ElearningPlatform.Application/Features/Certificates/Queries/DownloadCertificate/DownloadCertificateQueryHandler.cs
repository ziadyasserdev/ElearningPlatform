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

namespace ElearningPlatform.Application.Features.Certificates.Queries.DownloadCertificate
{
    public class DownloadCertificateQueryHandler
     : IRequestHandler<DownloadCertificateQuery, Result<DownloadCertificateDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public DownloadCertificateQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<DownloadCertificateDto>> Handle(
            DownloadCertificateQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<DownloadCertificateDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required.");
            }

            var userId = currentUserService.UserId;

            var certificate = await unitOfWork.Certificates.Query()
                .FirstOrDefaultAsync(x =>
                    x.Id == request.Id &&
                    x.StudentId == userId &&
                    !x.IsDeleted,
                    cancellationToken);

            if (certificate == null)
            {
                return Result<DownloadCertificateDto>.Failure(
                    ResultStatus.NotFound,
                    "Certificate not found.");
            }

            if (certificate.IsRevoked)
            {
                return Result<DownloadCertificateDto>.Failure(
                    ResultStatus.Forbidden,
                    "This certificate has been revoked.");
            }

            certificate.DownloadCount++;

            certificate.LastDownloadedAt = DateTime.Now;

            certificate.UpdatedAt = DateTime.Now;

            certificate.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<DownloadCertificateDto>.Success(
                new DownloadCertificateDto
                {
                    CertificateUrl = certificate.CertificateUrl,
                    FileName = $"{certificate.CertificateNumber}.pdf"
                });
        }
    }
}
