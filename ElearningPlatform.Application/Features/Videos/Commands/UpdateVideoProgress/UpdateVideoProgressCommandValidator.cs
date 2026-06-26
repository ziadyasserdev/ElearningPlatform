using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.UpdateVideoProgress
{
    public class UpdateVideoProgressCommandValidator : AbstractValidator<UpdateVideoProgressCommand>
    {
        public UpdateVideoProgressCommandValidator()
        {
            RuleFor(x => x.VideoId)
                .GreaterThan(0);

            RuleFor(x => x.CurrentTimeInSeconds)
                .GreaterThanOrEqualTo(0);
        }
    }
}
