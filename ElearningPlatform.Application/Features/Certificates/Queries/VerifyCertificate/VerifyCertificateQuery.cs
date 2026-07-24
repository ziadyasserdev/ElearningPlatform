using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Certificates.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Certificates.Queries.VerifyCertificate
{
    public class VerifyCertificateQuery
        : IRequest<Result<VerifyCertificateDto>>
    {
        public string VerificationCode { get; set; } = string.Empty;
    }
}