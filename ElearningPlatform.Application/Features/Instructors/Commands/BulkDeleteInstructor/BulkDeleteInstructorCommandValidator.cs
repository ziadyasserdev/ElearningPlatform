using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Commands.BulkDeleteInstructor
{
    public class BulkDeleteInstructorCommandValidator : AbstractValidator<BulkDeleteInstructorCommand>
    {
        public BulkDeleteInstructorCommandValidator()
        {
          
            RuleFor(x => x.InstructorIds)
                .NotEmpty()
                .WithMessage("InstructorIds list cannot be empty.");

          
        }
    }
}
