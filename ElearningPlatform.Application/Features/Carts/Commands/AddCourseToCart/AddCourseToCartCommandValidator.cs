using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Carts.Commands.AddCourseToCart
{
    public class AddCourseToCartValidator : AbstractValidator<AddCourseToCartCommand>
    {
        public AddCourseToCartValidator()
        {
            RuleFor(x => x.CourseId)
                .GreaterThan(0);
        }
    }
}
