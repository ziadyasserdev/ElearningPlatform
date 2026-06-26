using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Commands.EditCourse
{
    public class EditCourseCommandValidator : AbstractValidator<EditCourseCommand>
    {
        public EditCourseCommandValidator()
        {
            RuleFor(x => x.CourseId)
                .GreaterThan(0);

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .NotEmpty();

            RuleFor(x => x.ShortDescription)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.DiscountPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.DiscountPrice.HasValue);

            RuleFor(x => x)
                .Must(x => !x.DiscountPrice.HasValue || x.DiscountPrice < x.Price)
                .WithMessage("Discount must be less than price");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0);

            RuleFor(x => x.Language)
                .NotEmpty();
        }
    }
}
