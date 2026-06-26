using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.RestoreVideo
{
    public class RestoreVideoCommandValidator : AbstractValidator<RestoreVideoCommand>
    {
        public RestoreVideoCommandValidator()
        {
            RuleFor(x => x.VideoId)
                .GreaterThan(0)
                .WithMessage("VideoId must be greater than 0");
        }
    }
}
