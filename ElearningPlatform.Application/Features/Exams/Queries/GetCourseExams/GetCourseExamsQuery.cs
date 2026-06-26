using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Exams.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Queries.GetCourseExams
{
    public class GetCourseExamsQuery
        : IRequest<Result<List<ExamDto>>>
    {
        public int CourseId { get; set; }
    }
}
