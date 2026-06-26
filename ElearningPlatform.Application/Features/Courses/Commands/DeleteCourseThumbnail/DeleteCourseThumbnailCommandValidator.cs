using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Commands.DeleteCourseThumbnail
{
    public class DeleteCourseThumbnailCommandValidator
       : AbstractValidator<DeleteCourseThumbnailCommand>
    {
        public DeleteCourseThumbnailCommandValidator()
        {
            RuleFor(x => x.CourseId)
                .GreaterThan(0)
                .WithMessage("CourseId must be greater than zero.");
        }
    }
}
