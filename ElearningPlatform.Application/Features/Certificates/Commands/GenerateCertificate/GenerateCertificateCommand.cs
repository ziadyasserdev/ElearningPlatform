using ElearningPlatform.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Certificates.Commands.GenerateCertificate
{
    public class GenerateCertificateCommand : IRequest<Result<int>>
    {
        public int CourseId { get; set; }

        public GenerateCertificateCommand(int courseId)
        {
            CourseId = courseId;
        }
    }
}
