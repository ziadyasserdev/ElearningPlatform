using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.ReorderVideos
{
    public class ReorderVideosCommandValidator : AbstractValidator<ReorderVideosCommand>
    {
        public ReorderVideosCommandValidator()
        {
            RuleFor(x => x.SectionId)
                .GreaterThan(0);

            RuleFor(x => x.Videos)
                .NotEmpty();

            RuleForEach(x => x.Videos).ChildRules(video =>
            {
                video.RuleFor(v => v.VideoId)
                    .GreaterThan(0);

                video.RuleFor(v => v.NewOrder)
                    .GreaterThan(0);
            });
        }
    }
}
