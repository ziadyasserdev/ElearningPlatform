using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Exams.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Queriess.GetMyExamResult
{
    public class GetMyExamResultQuery
      : IRequest<Result<MyExamResultDto>>
    {
        public int AttemptId { get; set; }
    }
    public class MyExamResultDto
    {
        public int AttemptId { get; set; }

        public string ExamTitle { get; set; } = string.Empty;

        public int Score { get; set; }

        public decimal Percentage { get; set; }

        public bool IsPassed { get; set; }

        public int TotalQuestions { get; set; }

        public int CorrectAnswers { get; set; }

        public int WrongAnswers { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public TimeSpan Duration { get; set; }
    }
}
