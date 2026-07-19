using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Assignments.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Queries.GetTopStudents
{
    public class GetAssignmentTopStudentsQueryHandler
     : IRequestHandler<GetAssignmentTopStudentsQuery, Result<List<TopStudentDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetAssignmentTopStudentsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<TopStudentDto>>> Handle(
            GetAssignmentTopStudentsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<List<TopStudentDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

            var userId = currentUserService.UserId;

            var assignment = await unitOfWork.Assignments
                .Query()
                .Include(a => a.Course)
                .ThenInclude(c => c.Instructor)
                .FirstOrDefaultAsync(a =>
                    a.Id == request.AssignmentId &&
                    !a.IsDeleted,
                    cancellationToken);

            if (assignment is null)
            {
                return Result<List<TopStudentDto>>.Failure(
                    ResultStatus.NotFound,
                    "Assignment not found.");
            }

            if (assignment.Course.Instructor.UserId != userId)
            {
                return Result<List<TopStudentDto>>.Failure(
                    ResultStatus.Forbidden,
                    "You are not authorized to view top students.");
            }

            var students = await unitOfWork.Submissions
                .Query()
                .Where(s =>
                    s.AssignmentId == assignment.Id &&
                    !s.IsDeleted &&
                    s.Score.HasValue)
                .OrderByDescending(s => s.Score)
                .ThenBy(s => s.SubmittedAt)
                .Take(request.Count)
                .Select(s => new TopStudentDto
                {
                    StudentId = s.StudentId,
                    StudentName = s.Student.FullName,
                    ProfilePictureUrl = s.Student.ProfileImageUrl,
                    Score = s.Score,
                    SubmittedAt = s.SubmittedAt,
                    IsLate = s.SubmittedAt > assignment.DueDate
                })
                .ToListAsync(cancellationToken);

            return Result<List<TopStudentDto>>.Success(students);
        }
    }
}
