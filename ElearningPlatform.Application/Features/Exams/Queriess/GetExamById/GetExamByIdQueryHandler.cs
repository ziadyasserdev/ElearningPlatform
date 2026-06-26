using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Exams.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Queriess.GetExamById
{
    public class GetExamByIdQueryHandler
     : IRequestHandler<GetExamByIdQuery, Result<ExamDetailsDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetExamByIdQueryHandler(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<ExamDetailsDto>> Handle(
            GetExamByIdQuery request,
            CancellationToken cancellationToken)
        {



            var exam = await unitOfWork.Exams.Query()
                .Where(x => x.Id == request.Id)
                .Select(x => new ExamDetailsDto
                {
                    Id = x.Id,
                    CourseId = x.CourseId,
                    CourseTitle = x.Course.Title,

                    Title = x.Title,
                    Description = x.Description,
                    StartAt = x.StartAt,
                    EndAt = x.EndAt,
                    DurationMinutes = x.DurationMinutes,
                    TotalScore = x.TotalScore,
                    PassingScore = x.PassingScore,

                   

                    QuestionsCount = x.Questions.Count
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (exam == null)
            {
                return Result<ExamDetailsDto>.Failure(
                    ResultStatus.NotFound,
                    "Exam not found");
            }

            return Result<ExamDetailsDto>.Success(exam);
        }
    }
}
