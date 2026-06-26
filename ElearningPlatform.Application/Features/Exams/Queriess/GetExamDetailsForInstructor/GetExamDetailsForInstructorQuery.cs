using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Features.Exams.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Queriess.GetExamDetailsForInstructor
{
    public class GetExamDetailsForInstructorQuery
      : IRequest<Result<ExamInstructorDetailsDto>>
    {
        public int Id { get; set; }
    }
}
