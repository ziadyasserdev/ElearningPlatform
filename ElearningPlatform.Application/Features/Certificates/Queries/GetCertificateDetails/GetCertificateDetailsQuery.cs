using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Certificates.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Certificates.Queries.GetCertificateDetails
{
    public class GetCertificateDetailsQuery
       : IRequest<Result<CertificateDetailsDto>>
    {
        public int Id { get; set; }

        public GetCertificateDetailsQuery(int id)
        {
            Id = id;
        }
    }
}
