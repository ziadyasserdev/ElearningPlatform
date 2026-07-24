using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Certificates.Queries.GetCertificates
{
    public class GetCertificatesQueryValidator
        : AbstractValidator<GetCertificatesQuery>
    {
        public GetCertificatesQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100);

            RuleFor(x => x)
                .Must(x => !x.From.HasValue || !x.To.HasValue || x.From <= x.To)
                .WithMessage("'From' date must be earlier than or equal to 'To' date.");
        }
    }
}
