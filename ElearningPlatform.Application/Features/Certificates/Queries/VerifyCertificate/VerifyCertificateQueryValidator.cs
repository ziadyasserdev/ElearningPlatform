using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Certificates.Queries.VerifyCertificate
{
    public class VerifyCertificateQueryValidator
        : AbstractValidator<VerifyCertificateQuery>
    {
        public VerifyCertificateQueryValidator()
        {
            RuleFor(x => x.VerificationCode)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("Verification code is required.");
        }
    }
}
