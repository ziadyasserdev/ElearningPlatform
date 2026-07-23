using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Enums;
using ElearningPlatform.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Certificates.Commands.GenerateCertificate
{
    public class GenerateCertificateCommandHandler
          : IRequestHandler<GenerateCertificateCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GenerateCertificateCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(
            GenerateCertificateCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<int>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required.");
            }

            if (!currentUserService.IsInRole("Student"))
            {
                return Result<int>.Failure(
                    ResultStatus.Forbidden,
                    "Only students can generate certificates.");
            }

            var userId = currentUserService.UserId;

            var enrollment = await unitOfWork.Enrollments.Query()
                .FirstOrDefaultAsync(x =>
                    x.StudentId == userId &&
                    x.CourseId == request.CourseId,
                    cancellationToken);

            if (enrollment == null)
            {
                return Result<int>.Failure(
                    ResultStatus.NotFound,
                    "Enrollment not found.");
            }

            if (enrollment.Status != EnrollmentStatus.Completed)
            {
                return Result<int>.Failure(
                    ResultStatus.Failure,
                    "You must complete the course before generating a certificate.");
            }

            var exists = await unitOfWork.Certificates.Query()
                .AnyAsync(x =>
                    x.StudentId == userId &&
                    x.CourseId == request.CourseId &&
                    !x.IsDeleted,
                    cancellationToken);

            if (exists)
            {
                return Result<int>.Failure(
                    ResultStatus.Conflict,
                    "Certificate already exists.");
            }

            var currentYear = DateTime.UtcNow.Year;

            var lastCertificate = await unitOfWork.Certificates.Query()
                .Where(x => x.CertificateNumber.StartsWith($"CERT-{currentYear}-"))
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken);

            int nextNumber = 1;

            if (lastCertificate != null)
            {
                var parts = lastCertificate.CertificateNumber.Split('-');

                if (parts.Length == 3 &&
                    int.TryParse(parts[2], out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            var certificateNumber =
                $"CERT-{currentYear}-{nextNumber:D6}";

            var verificationCode =
                Guid.NewGuid()
                    .ToString("N")[..12]
                    .ToUpper();

            var certificate = new Certificate
            {
                StudentId = userId!,

                CourseId = request.CourseId,

                CertificateNumber = certificateNumber,

                VerificationCode = verificationCode,

            
                CertificateUrl = string.Empty,

                IssuedAt = DateTime.Now,

                IsRevoked = false,

                DownloadCount = 0,

                LastDownloadedAt = null,

                CreatedAt = DateTime.Now,

                CreatedBy = currentUserService.UserName
            };

            await unitOfWork.Certificates.AddAsync(certificate);

            await unitOfWork.SaveAsync();

            return Result<int>.Success(certificate.Id);
        }
    }
}
