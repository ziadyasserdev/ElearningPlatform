using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Videos.Commands.DeleteVideo
{
    public class DeleteVideoCommandValidator : AbstractValidator<DeleteVideoCommand>
    {
        public DeleteVideoCommandValidator()
        {
            RuleFor(x => x.Id)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Video Id is required")
                .GreaterThan(0).WithMessage("Video Id must be greater than 0");
        }
    }
}
