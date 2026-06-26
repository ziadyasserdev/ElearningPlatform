using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Exams.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Exams.Queriess.GetFailedStudents
{
    public class GetFailedStudentsQueryHandler
    : IRequestHandler<GetFailedStudentsQuery, Result<List<FailedStudentDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetFailedStudentsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<FailedStudentDto>>> Handle(
            GetFailedStudentsQuery request,
            CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<List<FailedStudentDto>>.Failure(ResultStatus.Unauthorized, "Authentication required");
            var userId = currentUserService.UserId;

            var result = await unitOfWork.ExamAttempts.Query()
                .Where(x =>
                    x.ExamId == request.Id &&
                    x.Exam.Course.Instructor.UserId == userId &&
                    x.Score < x.Exam.PassingScore)
                .Select(x => new FailedStudentDto
                {
                    StudentName = x.Student.FullName,
                    Email = x.Student.Email,
                    Score = x.Score,
                    EndTime = x.EndTime
                })
                .ToListAsync(cancellationToken);

            return Result<List<FailedStudentDto>>.Success(result);
        }
    }
}
