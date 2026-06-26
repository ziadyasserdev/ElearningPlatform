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

namespace ElearningPlatform.Application.Features.Exams.Queriess.GetTopStudents
{
    public class GetTopStudentsQueryHandler
       : IRequestHandler<GetTopStudentssQuery, Result<List<TopStudentDtoo>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetTopStudentsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<TopStudentDtoo>>> Handle(
            GetTopStudentssQuery request,
            CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<List<TopStudentDtoo>>.Failure(ResultStatus.Unauthorized, "Authentication required");
            var userId = currentUserService.UserId;

            var result = await unitOfWork.ExamAttempts.Query()
                .Where(x =>
                    x.ExamId == request.Id &&
                    x.Exam.Course.Instructor.UserId == userId)
                .OrderByDescending(x => x.Score)
                .Select(x => new TopStudentDtoo
                {
                    StudentName = x.Student.FullName,
                    Email = x.Student.Email,
                    Score = x.Score,
                    AttemptId = x.Id
                })
                .Take(10)
                .ToListAsync(cancellationToken);

            return Result<List<TopStudentDtoo>>.Success(result);
        }
    }


}
