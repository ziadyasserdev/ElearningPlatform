using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Certificates.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Certificates.Queries.GetCertificates
{
    public class GetCertificatesQuery
     : IRequest<Result<PaginatedResult<AdminCertificateDto>>>
    {
        public string? Search { get; set; }

        public bool? IsRevoked { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }

}
