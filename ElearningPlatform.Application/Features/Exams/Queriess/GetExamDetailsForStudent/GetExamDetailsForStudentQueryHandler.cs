using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Exams.Dtos;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Queriess.GetExamDetailsForStudent
{
    public class GetExamDetailsForStudentQueryHandler
         : IRequestHandler<GetExamDetailsForStudentQuery,
             Result<ExamDetailsForStudentDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetExamDetailsForStudentQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<ExamDetailsForStudentDto>> Handle(
            GetExamDetailsForStudentQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<ExamDetailsForStudentDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required");
            }

            var userId = currentUserService.UserId;

            var exam = await unitOfWork.Exams.Query()
                .Where(x =>
                    x.Id == request.Id)
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    x.Description,
                    x.DurationMinutes,
                    x.TotalScore,
                    x.PassingScore,
                    x.CourseId,
                    x.StartAt,
                    x.EndAt,
                    QuestionsCount = x.Questions.Count()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (exam is null)
            {
                return Result<ExamDetailsForStudentDto>.Failure(
                    ResultStatus.NotFound,
                    "Exam not found");
            }

            var enrolled = await unitOfWork.Enrollments.Query()
                .AnyAsync(x =>
                    x.CourseId == exam.CourseId &&
                    x.StudentId == userId,
                    cancellationToken);

            if (!enrolled)
            {
                return Result<ExamDetailsForStudentDto>.Failure(
                    ResultStatus.Forbidden,
                    "You are not enrolled in this course");
            }

            var attemptsCount = await unitOfWork.ExamAttempts.Query()
                .CountAsync(x =>
                    x.ExamId == exam.Id &&
                    x.StudentId == userId,
                    cancellationToken);

            var hasStarted = await unitOfWork.ExamAttempts.Query()
                .AnyAsync(x =>
                    x.ExamId == exam.Id &&
                    x.StudentId == userId,
                    cancellationToken);

            var hasCompleted = await unitOfWork.ExamAttempts.Query()
                .AnyAsync(x =>
                    x.ExamId == exam.Id &&
                    x.StudentId == userId &&
                    x.Status == Domain.Enums.ExamAttemptStatus.Completed,
                    cancellationToken);

            var dto = new ExamDetailsForStudentDto
            {
                Id = exam.Id,
                Title = exam.Title,
                Description = exam.Description,
                StartAt = exam.StartAt,
                EndAt = exam.EndAt,
                DurationMinutes = exam.DurationMinutes,
                TotalScore = exam.TotalScore,
                PassingScore = exam.PassingScore,
                QuestionsCount = exam.QuestionsCount,
                AttemptsCount = attemptsCount,
                HasStarted = hasStarted,
                HasCompleted = hasCompleted
            };

            return Result<ExamDetailsForStudentDto>.Success(dto);
        }
    }
}
