using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Queriess.GetExamStatistics
{
    public class GetExamStatisticsQueryValidator
       : AbstractValidator<GetExamStatisticsQuery>
    {
        public GetExamStatisticsQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Exam Id must be greater than 0");
        }
    }
}
