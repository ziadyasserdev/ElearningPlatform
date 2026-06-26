using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Commands.BulkRestoreInstructor
{
    public class BulkRestoreInstructorCommandValidator
       : AbstractValidator<BulkRestoreInstructorCommand>
    {
        public BulkRestoreInstructorCommandValidator()
        {
            RuleFor(x => x.InstructorIds)
                .NotEmpty().WithMessage("Instructor IDs are required.");

          
        }
    }
}
