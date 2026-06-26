using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Commands.EditInstructor
{
    public class EditInstructorCommandValidator : AbstractValidator<EditInstructorCommand>
    {
        public EditInstructorCommandValidator()
        {

            RuleFor(x => x.Specialization)
                .NotEmpty().WithMessage("Specialization is required")
                .MaximumLength(100).WithMessage("Specialization must not exceed 100 characters");


            RuleFor(x => x.ExperienceYears)
                .GreaterThanOrEqualTo(0)
                .When(x => x.ExperienceYears.HasValue)
                .WithMessage("Experience years must be 0 or greater");


            RuleFor(x => x.LinkedInUrl)
                .Must(BeAValidUrl)
                .When(x => !string.IsNullOrEmpty(x.LinkedInUrl))
                .WithMessage("LinkedIn URL must be valid");
            RuleFor(x => x.Bio)
              .MaximumLength(500).WithMessage("Bio cannot exceed 500 characters");

            RuleFor(x => x.WebsiteUrl)
                .Must(BeAValidUrl)
                .When(x => !string.IsNullOrEmpty(x.WebsiteUrl))
                .WithMessage("Website URL must be valid");
        }


        private bool BeAValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
}
