using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.BulkRestoreVideos
{
    public class BulkRestoreVideosCommandValidator : AbstractValidator<BulkRestoreVideosCommand>
    {
        public BulkRestoreVideosCommandValidator()
        {
            RuleFor(x => x.VideoIds)
                .NotEmpty().WithMessage("VideoIds cannot be empty");

            RuleFor(x => x.VideoIds)
                .Must(x => x.Distinct().Count() == x.Count)
                .WithMessage("Duplicate VideoIds are not allowed");

            RuleForEach(x => x.VideoIds)
                .GreaterThan(0);
        }
    }
}
