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

namespace ElearningPlatform.Application.Features.Certificates.Queries.GetCertificateDetails
{
    public class GetCertificateDetailsQueryHandler
        : IRequestHandler<GetCertificateDetailsQuery, Result<CertificateDetailsDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetCertificateDetailsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<CertificateDetailsDto>> Handle(
            GetCertificateDetailsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<CertificateDetailsDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required.");
            }

            var userId = currentUserService.UserId;

            var certificate = await unitOfWork.Certificates.Query()
                .Where(x =>
                    x.Id == request.Id &&
                    !x.IsDeleted &&
                    x.StudentId == userId)
                .Select(x => new CertificateDetailsDto
                {
                    Id = x.Id,

                    CourseId = x.CourseId,

                    CourseTitle = x.Course.Title,

                    InstructorName = x.Course.Instructor.User.FullName,

                    StudentName = x.Student.FullName,

                    CertificateNumber = x.CertificateNumber,

                    VerificationCode = x.VerificationCode,

                    CertificateUrl = x.CertificateUrl,

                    IssuedAt = x.IssuedAt,

                    IsRevoked = x.IsRevoked,

                    RevokedAt = x.RevokedAt,

                    RevokedReason = x.RevokedReason,

                    DownloadCount = x.DownloadCount,

                    LastDownloadedAt = x.LastDownloadedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (certificate == null)
            {
                return Result<CertificateDetailsDto>.Failure(
                    ResultStatus.NotFound,
                    "Certificate not found.");
            }

            return Result<CertificateDetailsDto>.Success(certificate);
        }
    }
}
