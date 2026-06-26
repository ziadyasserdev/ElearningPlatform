using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Courses.Commands.CreateCourse
{
    public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
    {
        public CreateCourseCommandValidator()
        {
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
                .WithMessage("Discount price must be less than original price");

            RuleFor(x => x.DiscountEndDate)
                .GreaterThan(DateTime.UtcNow)
                .When(x => x.DiscountEndDate.HasValue);

            RuleFor(x => x.CategoryId)
                .GreaterThan(0);

            RuleFor(x => x.InstructorId)
                .NotEmpty();

            RuleFor(x => x.Language)
                .NotEmpty()
                .MaximumLength(10);
        }
    }
}
