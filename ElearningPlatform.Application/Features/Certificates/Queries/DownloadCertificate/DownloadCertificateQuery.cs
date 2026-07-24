using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Certificates.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Certificates.Queries.DownloadCertificate
{
    public class DownloadCertificateQuery
       : IRequest<Result<DownloadCertificateDto>>
    {
        public int Id { get; set; }

        public DownloadCertificateQuery(int id)
        {
            Id = id;
        }
    }
}
