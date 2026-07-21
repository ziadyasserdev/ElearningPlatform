using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Carts.Commands.RemoveCourseFromCart
{
    public class RemoveCourseFromCartValidator : AbstractValidator<RemoveCourseFromCartCommand>
    {
        public RemoveCourseFromCartValidator()
        {
            RuleFor(x => x.CourseId)
                .GreaterThan(0);
        }
    }
}
