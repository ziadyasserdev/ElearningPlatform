using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Exams.Queriess.GetMyExamResult;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Queriess.GetMyExamResults
{
    public class GetMyExamResultsQuery
          : IRequest<Result<List<MyExamResultsDto>>>
    {
    }
    public class MyExamResultsDto
    {
        public int AttemptId { get; set; }

        public int ExamId { get; set; }

        public string ExamTitle { get; set; } = string.Empty;

        public int Score { get; set; }

        public decimal Percentage { get; set; }

        public bool IsPassed { get; set; }

        public int AttemptNumber { get; set; }

        public DateTime SubmittedAt { get; set; }
    }
}
