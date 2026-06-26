using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Commands.CreateExam
{
    public class CreateExamCommandHandler
       : IRequestHandler<CreateExamCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public CreateExamCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            CreateExamCommand request,
            CancellationToken cancellationToken)
        {

            if(!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "Authentication required");
            var userId = currentUserService.UserId;

            var course = await unitOfWork.Courses.Query()
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(
                    c => c.Id == request.CourseId &&
                         !c.IsDeleted,
                    cancellationToken);

            if (course == null)
            {
                return Result<string>.Failure(
                    ResultStatus.NotFound,
                    "Course not found");
            }

            if (course.Instructor.UserId != userId)
            {
                return Result<string>.Failure(
                    ResultStatus.Forbidden,
                    "Not allowed");
            }

            var exists = await unitOfWork.Exams.Query()
                .AnyAsync(x =>
                    x.CourseId == request.CourseId &&
                    x.Title == request.Title,
                    cancellationToken);

            if (exists)
            {
                return Result<string>.Failure(
                    ResultStatus.Conflict,
                    "Exam title already exists");
            }

            var exam = new Exam
            {
                CourseId = request.CourseId,
                Title = request.Title,
                Description = request.Description,
                StartAt = request.StartAt,
                EndAt = request.EndAt,
                DurationMinutes = request.DurationMinutes,
                TotalScore = request.TotalScore,
                PassingScore = request.PassingScore,

              

                CreatedAt = DateTime.Now,
                CreatedBy = currentUserService.UserName
            };

            await unitOfWork.Exams.AddAsync(exam);

            await unitOfWork.SaveAsync();

            return Result<string>.Success(
               
                "Exam created successfully");
        }
    }
}
