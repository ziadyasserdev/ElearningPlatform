using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Queriess.GetExamDetailsForStudent
{
    public class GetExamDetailsForStudentQueryValidator
       : AbstractValidator<GetExamDetailsForStudentQuery>
    {
        public GetExamDetailsForStudentQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Exam Id must be greater than 0.");
        }
    }
}
