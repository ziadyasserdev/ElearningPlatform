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

namespace ElearningPlatform.Application.Features.Certificates.Queries.GetMyCertificates
{
    public class GetMyCertificatesQueryHandler
         : IRequestHandler<GetMyCertificatesQuery, Result<List<MyCertificateDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetMyCertificatesQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<MyCertificateDto>>> Handle(
            GetMyCertificatesQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<List<MyCertificateDto>>
                    .Failure(ResultStatus.Unauthorized,
                        "Authentication required.");
            }

            var userId = currentUserService.UserId;

            var certificates = await unitOfWork.Certificates.Query()
                .Where(x =>
                    x.StudentId == userId &&
                    !x.IsDeleted)
                .OrderByDescending(x => x.IssuedAt)
                .Select(x => new MyCertificateDto
                {
                    Id = x.Id,

                    CourseId = x.CourseId,

                    CourseTitle = x.Course.Title,

                    InstructorName = x.Course.Instructor.User.FullName,

                    CertificateNumber = x.CertificateNumber,

                    IssuedAt = x.IssuedAt,

                    CertificateUrl = x.CertificateUrl,

                    IsRevoked = x.IsRevoked,

                    DownloadCount = x.DownloadCount
                })
                .ToListAsync(cancellationToken);

            return Result<List<MyCertificateDto>>
                .Success(certificates);
        }
    }
}
