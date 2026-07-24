using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Certificates.Commands.RestoreCertificate
{
    public class RestoreCertificateCommandHandler
      : IRequestHandler<RestoreCertificateCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public RestoreCertificateCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            RestoreCertificateCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<string>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required.");
            }

            if (!currentUserService.IsInRole("Admin"))
            {
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "Only administrators can restore certificates.");
            }

            var certificate = await unitOfWork.Certificates.Query()
                .FirstOrDefaultAsync(x =>
                    x.Id == request.Id &&
                    !x.IsDeleted,
                    cancellationToken);

            if (certificate == null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Certificate not found.");
            }

            if (!certificate.IsRevoked)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Certificate is already active.");
            }

            certificate.IsRevoked = false;
            certificate.RevokedAt = null;
            certificate.RevokedReason = null;

            certificate.UpdatedAt = DateTime.Now;
            certificate.UpdatedBy = currentUserService.UserName;

            await unitOfWork.SaveAsync();

            return Result<string>.Success(
                "Certificate restored successfully.");
        }
    }
}
