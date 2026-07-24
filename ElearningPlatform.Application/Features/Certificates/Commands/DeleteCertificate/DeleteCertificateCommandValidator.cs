using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Certificates.Commands.DeleteCertificate
{
    public class DeleteCertificateCommandValidator
          : AbstractValidator<DeleteCertificateCommand>
    {
        public DeleteCertificateCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Certificate Id must be greater than zero.");
        }
    }
}
