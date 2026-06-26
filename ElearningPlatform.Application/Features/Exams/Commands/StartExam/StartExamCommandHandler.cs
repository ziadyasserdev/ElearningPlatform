using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Enums;
using ElearningPlatform.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Commands.StartExam
{
    public class StartExamCommandHandler
       : IRequestHandler<StartExamCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public StartExamCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(
            StartExamCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<int>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required");
            }

            var userId = currentUserService.UserId;
            var now = DateTime.UtcNow;

            var exam = await unitOfWork.Exams.Query()
                .FirstOrDefaultAsync(
                    x => x.Id == request.ExamId,
                    cancellationToken);

            if (exam is null)
            {
                return Result<int>.Failure(
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
                return Result<int>.Failure(
                    ResultStatus.Forbidden,
                    "You are not enrolled in this course");
            }

           
            if (exam.StartAt.HasValue &&
                now < exam.StartAt.Value)
            {
                return Result<int>.Failure(
                    ResultStatus.Failure,
                    "Exam has not started yet");
            }

           
            if (exam.EndAt.HasValue &&
                now > exam.EndAt.Value)
            {
                return Result<int>.Failure(
                    ResultStatus.Failure,
                    "Exam has ended");
            }

           
            var activeAttempt = await unitOfWork.ExamAttempts.Query()
                .FirstOrDefaultAsync(x =>
                    x.ExamId == request.ExamId &&
                    x.StudentId == userId &&
                    x.Status == ExamAttemptStatus.InProgress,
                    cancellationToken);

            if (activeAttempt != null)
            {
                return Result<int>.Success(activeAttempt.Id);
            }

            var attemptCount = await unitOfWork.ExamAttempts.Query()
                .CountAsync(x =>
                    x.ExamId == request.ExamId &&
                    x.StudentId == userId,
                    cancellationToken);

            var attempt = new ExamAttempt
            {
                StudentId = userId,
                ExamId = request.ExamId,

                StartTime = now,

                AttemptNumber = attemptCount + 1,

                Status = ExamAttemptStatus.InProgress,

                Score = 0,
                Percentage = 0,
                IsPassed = false,

                CreatedAt = now,
                CreatedBy = currentUserService.UserName
            };

            await unitOfWork.ExamAttempts.AddAsync(attempt);

            await unitOfWork.SaveAsync();

            return Result<int>.Success(attempt.Id);
        }
    }
}
