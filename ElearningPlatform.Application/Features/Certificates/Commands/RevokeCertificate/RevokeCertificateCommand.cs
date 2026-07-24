using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Certificates.Commands.RevokeCertificate
{
    public class RevokeCertificateCommand : IRequest<Result<string>>
    {
        public int Id { get; set; }

        public string Reason { get; set; } = string.Empty;
    }
}
