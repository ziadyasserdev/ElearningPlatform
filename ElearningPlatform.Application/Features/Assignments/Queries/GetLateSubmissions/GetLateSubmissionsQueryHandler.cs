using ElearningPlatform.Application.Common.PaginatedResults;
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

namespace ElearningPlatform.Application.Features.Assignments.Queries.GetLateSubmissions
{
    public class GetLateSubmissionsQueryHandler
          : IRequestHandler<GetLateSubmissionsQuery,
              Result<PaginatedResult<LateSubmissionDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetLateSubmissionsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<PaginatedResult<LateSubmissionDto>>> Handle(
            GetLateSubmissionsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<PaginatedResult<LateSubmissionDto>>.Failure(
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
                return Result<PaginatedResult<LateSubmissionDto>>.Failure(
                    ResultStatus.NotFound,
                    "Assignment not found.");
            }

            if (assignment.Course.Instructor.UserId != userId)
            {
                return Result<PaginatedResult<LateSubmissionDto>>.Failure(
                    ResultStatus.Forbidden,
                    "You are not authorized to view these submissions.");
            }

            var query = unitOfWork.Submissions
                .Query()
                .Where(s =>
                    s.AssignmentId == assignment.Id &&
                    !s.IsDeleted &&
                    s.SubmittedAt > assignment.DueDate);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim().ToLower();

                query = query.Where(s =>
                    s.Student.FullName.ToLower().Contains(search) ||
                    s.Student.Email.ToLower().Contains(search));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var submissions = await query
                .OrderByDescending(s => s.SubmittedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(s => new LateSubmissionDto
                {
                    SubmissionId = s.Id,
                    StudentId = s.StudentId,
                    StudentName = s.Student.FullName,
                    ProfilePictureUrl = s.Student.ProfileImageUrl,
                    FileUrl = s.FileUrl,
                    Score = s.Score,
                    Status = s.Status,
                    SubmittedAt = s.SubmittedAt,
                    DueDate = assignment.DueDate,
                    LateDays  = (s.SubmittedAt.Date - assignment.DueDate.Date).Days
                })
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<LateSubmissionDto>(
                submissions,
                request.PageNumber,
                request.PageSize,
                totalCount);

            return Result<PaginatedResult<LateSubmissionDto>>
                .Success(result);
        }
    }
}
