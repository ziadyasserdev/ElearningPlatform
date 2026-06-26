using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Queries.DownloadVideo
{
    public class DownloadVideoQueryValidator : AbstractValidator<DownloadVideoQuery>
    {
        public DownloadVideoQueryValidator()
        {
            RuleFor(x => x.LessonId)
                .GreaterThan(0)
                .WithMessage("LessonId must be greater than 0");

            RuleFor(x => x.VideoId)
                .GreaterThan(0)
                .WithMessage("VideoId must be greater than 0");
        }
    }
}
