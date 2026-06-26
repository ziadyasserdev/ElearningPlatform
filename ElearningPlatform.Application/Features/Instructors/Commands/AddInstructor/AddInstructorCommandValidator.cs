using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Commands.AddInstructor
{
    public class AddInstructorCommandValidator : AbstractValidator<AddInstructorCommand>
    {
        public AddInstructorCommandValidator()
        {
           
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters");

          
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

         
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^\+?[0-9]{10,15}$")
                .WithMessage("Invalid phone number format");

          
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");

        
            RuleFor(x => x.Bio)
                .MaximumLength(500).WithMessage("Bio cannot exceed 500 characters");

         
            RuleFor(x => x.Specialization)
                .NotEmpty().WithMessage("Specialization is required")
                .MaximumLength(100).WithMessage("Specialization cannot exceed 100 characters");

         
            RuleFor(x => x.ExperienceYears)
                .GreaterThanOrEqualTo(0).WithMessage("Experience years must be at least 0");

            
            RuleFor(x => x.LinkedInUrl)
                .Matches(@"^(https?://)?(www\.)?linkedin\.com/.*$").When(x => !string.IsNullOrEmpty(x.LinkedInUrl))
                .WithMessage("Invalid LinkedIn URL");

            RuleFor(x => x.WebsiteUrl)
                .Matches(@"^(https?://)?(www\.)?[\w\-]+(\.[\w\-]+)+(/[\w\-./?%&=]*)?$").When(x => !string.IsNullOrEmpty(x.WebsiteUrl))
                .WithMessage("Invalid website URL");
        }
    }
}
