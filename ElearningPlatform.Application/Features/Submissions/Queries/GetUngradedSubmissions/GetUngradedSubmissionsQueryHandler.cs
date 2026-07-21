using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Submissions.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Queries.GetUngradedSubmissions
{
    public class GetUngradedSubmissionsQueryHandler
        : IRequestHandler<
            GetUngradedSubmissionsQuery,
            Result<PaginatedResult<UngradedSubmissionDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetUngradedSubmissionsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<PaginatedResult<UngradedSubmissionDto>>> Handle(
            GetUngradedSubmissionsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<PaginatedResult<UngradedSubmissionDto>>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

            var userId = currentUserService.UserId;

           
            var assignmentExists = await unitOfWork.Assignments
                .Query()
                .AnyAsync(a =>
                    a.Id == request.AssignmentId &&
                    !a.IsDeleted &&
                    a.Course.Instructor.UserId == userId,
                    cancellationToken);

            if (!assignmentExists)
            {
                return Result<PaginatedResult<UngradedSubmissionDto>>.Failure(
                    ResultStatus.NotFound,
                    "Assignment not found.");
            }

            var query = unitOfWork.Submissions
                .Query()
                .AsNoTracking()
                .Where(s =>
                    s.AssignmentId == request.AssignmentId &&
                    !s.IsDeleted &&
                    s.Status != Domain.Enums.SubmissionStatus.Graded);

          
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim().ToLower();

                query = query.Where(s =>
                    s.Student.FullName.ToLower().Contains(search) ||
                    s.Student.Email!.ToLower().Contains(search));
            }

          
            if (request.IsLate.HasValue)
            {
                query = query.Where(s =>
                    s.IsLate == request.IsLate.Value);
            }

          
            var totalCount = await query
                .CountAsync(cancellationToken);

           
            var submissions = await query
                .OrderByDescending(s => s.SubmittedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(s => new UngradedSubmissionDto
                {
                    Id = s.Id,

                    AssignmentId = s.AssignmentId,

                    AssignmentTitle = s.Assignment.Title,

                    StudentId = s.StudentId,

                    StudentName = s.Student.FullName,

                    StudentEmail = s.Student.Email,

                    FileName = s.FileName,

                    FileSize = s.FileSize,

                    Comment = s.Comment,

                    IsLate = s.IsLate,

                   
                    SubmittedAt = s.SubmittedAt
                })
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<UngradedSubmissionDto>(
                submissions,
                totalCount,
                request.PageNumber,
                request.PageSize);

            return Result<PaginatedResult<UngradedSubmissionDto>>
                .Success(result);
        }
    }
}
