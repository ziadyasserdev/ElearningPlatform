using ElearningPlatform.Application.Common.PaginatedResults;
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

namespace ElearningPlatform.Application.Features.Certificates.Queries.GetCertificates
{
    public class GetCertificatesQueryHandler
    : IRequestHandler<GetCertificatesQuery, Result<PaginatedResult<AdminCertificateDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetCertificatesQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<PaginatedResult<AdminCertificateDto>>> Handle(
            GetCertificatesQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<PaginatedResult<AdminCertificateDto>>
                    .Failure(
                        ResultStatus.Unauthorized,
                        "Authentication required.");
            }

            if (!currentUserService.IsInRole("Admin"))
            {
                return Result<PaginatedResult<AdminCertificateDto>>
                    .Failure(
                        ResultStatus.Forbidden,
                        "Only administrators can access certificates.");
            }

            var query = unitOfWork.Certificates.Query()
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x =>
                    x.Student.FullName.Contains(request.Search) ||
                    x.Student.Email.Contains(request.Search) ||
                    x.Course.Title.Contains(request.Search) ||
                    x.CertificateNumber.Contains(request.Search));
            }

            if (request.IsRevoked.HasValue)
            {
                query = query.Where(x =>
                    x.IsRevoked == request.IsRevoked.Value);
            }

            if (request.From.HasValue)
            {
                query = query.Where(x =>
                    x.IssuedAt >= request.From.Value);
            }

            if (request.To.HasValue)
            {
                query = query.Where(x =>
                    x.IssuedAt <= request.To.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var certificates = await query
                .OrderByDescending(x => x.IssuedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new AdminCertificateDto
                {
                    Id = x.Id,

                    StudentName = x.Student.FullName,

                    StudentEmail = x.Student.Email,

                    CourseTitle = x.Course.Title,

                    InstructorName = x.Course.Instructor.User.FullName,

                    CertificateNumber = x.CertificateNumber,

                    IssuedAt = x.IssuedAt,

                    IsRevoked = x.IsRevoked,

                    DownloadCount = x.DownloadCount
                })
                .ToListAsync(cancellationToken);

       

            return Result<PaginatedResult<AdminCertificateDto>>
                .Success(new PaginatedResult<AdminCertificateDto>(certificates,
                request.PageNumber,request.PageSize,totalCount));
        }
    }
}
