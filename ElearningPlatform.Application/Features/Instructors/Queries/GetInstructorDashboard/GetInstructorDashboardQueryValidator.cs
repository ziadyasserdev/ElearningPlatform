using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetInstructorDashboard
{
    public class GetInstructorDashboardQueryValidator
        : AbstractValidator<GetInstructorDashboardQuery>
    {
        public GetInstructorDashboardQueryValidator()
        {
            RuleFor(x => x.InstructorId)
                .GreaterThan(0);
        }
    }
}
