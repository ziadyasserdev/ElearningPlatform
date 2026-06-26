using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Queries.GetVideoById
{
    public class GetVideoByIdQueryValidator : AbstractValidator<GetVideoByIdQuery>
    {
        public GetVideoByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Video Id is required")
                .GreaterThan(0).WithMessage("Video Id must be greater than 0");
        }
    }
}
