using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Certificates.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Certificates.Queries.VerifyCertificate
{
    public class VerifyCertificateQueryHandler
         : IRequestHandler<VerifyCertificateQuery, Result<VerifyCertificateDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public VerifyCertificateQueryHandler(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<VerifyCertificateDto>> Handle(
            VerifyCertificateQuery request,
            CancellationToken cancellationToken)
        {
            var certificate = await unitOfWork.Certificates.Query()
                .Where(x =>
                    x.VerificationCode == request.VerificationCode &&
                    !x.IsDeleted)
                .Select(x => new VerifyCertificateDto
                {
                    IsValid = !x.IsRevoked,

                    IsRevoked = x.IsRevoked,

                    StudentName = x.Student.FullName,

                    CourseTitle = x.Course.Title,

                    InstructorName = x.Course.Instructor.User.FullName,

                    CertificateNumber = x.CertificateNumber,

                    IssuedAt = x.IssuedAt,

                    RevokedAt = x.RevokedAt,

                    RevokedReason = x.RevokedReason
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (certificate == null)
            {
                return Result<VerifyCertificateDto>.Failure(
                    ResultStatus.NotFound,
                    "Certificate not found.");
            }

            return Result<VerifyCertificateDto>.Success(certificate);
        }
    }
}
