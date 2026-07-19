using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Assignments.Dtos;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Assignments.Queries.GetAssignmentStatistics
{
    public class GetAssignmentStatisticsQueryHandler
      : IRequestHandler<GetAssignmentStatisticsQuery, Result<AssignmentStatisticsDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetAssignmentStatisticsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<AssignmentStatisticsDto>> Handle(
            GetAssignmentStatisticsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<AssignmentStatisticsDto>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

            var userId = currentUserService.UserId;

            var assignment = await unitOfWork.Assignments
                .Query()
                .Include(a => a.Course)
                    .ThenInclude(c => c.Instructor)
                .FirstOrDefaultAsync(a =>
                    a.Id == request.Id &&
                    !a.IsDeleted,
                    cancellationToken);

            if (assignment is null)
            {
                return Result<AssignmentStatisticsDto>.Failure(
                    ResultStatus.NotFound,
                    "Assignment not found.");
            }

            if (assignment.Course.Instructor.UserId != userId)
            {
                return Result<AssignmentStatisticsDto>.Failure(
                    ResultStatus.Forbidden,
                    "You are not authorized to view this assignment statistics.");
            }

            var totalStudents = await unitOfWork.Enrollments
                .Query()
                .CountAsync(e =>
                    e.CourseId == assignment.CourseId &&
                    !e.IsDeleted,
                    cancellationToken);

            var submissions = await unitOfWork.Submissions
                .Query()
                .Where(s =>
                    s.AssignmentId == assignment.Id &&
                    !s.IsDeleted)
                .ToListAsync(cancellationToken);

            var submittedStudents = submissions.Count;

            var pendingStudents = Math.Max(0, totalStudents - submittedStudents);

            var lateSubmissions = submissions.Count(s =>
                s.SubmittedAt > assignment.DueDate);

            decimal averageScore = 0;
            int highestScore = 0;
            int lowestScore = 0;

            if (submittedStudents > 0)
            {
                averageScore = (decimal)submissions.Average(s => s.Score)!;
                highestScore = (int)submissions.Max(s => s.Score)!;
                lowestScore = (int)submissions.Min(s => s.Score)!;
            }

            decimal submissionRate = totalStudents == 0
                ? 0
                : Math.Round((decimal)submittedStudents * 100 / totalStudents, 2);

            var dto = new AssignmentStatisticsDto
            {
                TotalStudents = totalStudents,
                SubmittedStudents = submittedStudents,
                PendingStudents = pendingStudents,
                LateSubmissions = lateSubmissions,
                AverageScore = averageScore,
                HighestScore = highestScore,
                LowestScore = lowestScore,
                SubmissionRate = submissionRate
            };

            return Result<AssignmentStatisticsDto>.Success(dto);
        }
    }
}

