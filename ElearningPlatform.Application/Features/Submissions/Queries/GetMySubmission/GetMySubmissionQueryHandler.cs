using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Submissions.Dtos;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Queries.GetMySubmission
{
    public class GetMySubmissionQueryHandler
          : IRequestHandler<GetMySubmissionQuery, Result<MySubmissionDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMySubmissionQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<MySubmissionDto>> Handle(
            GetMySubmissionQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<MySubmissionDto>.Failure(
                    ResultStatus.Unauthorized,
                    "User is not authenticated.");
            }

            var studentId = currentUserService.UserId;

            var assignment = await unitOfWork.Assignments
                .Query()
                .FirstOrDefaultAsync(a =>
                    a.Id == request.AssignmentId &&
                    !a.IsDeleted,
                    cancellationToken);

            if (assignment is null)
            {
                return Result<MySubmissionDto>.Failure(
                    ResultStatus.NotFound,
                    "Assignment not found.");
            }

            var isEnrolled = await unitOfWork.Enrollments
                .Query()
                .AnyAsync(e =>
                    e.CourseId == assignment.CourseId &&
                    e.StudentId == studentId &&
                    e.Status == EnrollmentStatus.Active,
                    cancellationToken);

            if (!isEnrolled)
            {
                return Result<MySubmissionDto>.Failure(
                    ResultStatus.Forbidden,
                    "You are not enrolled in this course.");
            }

            var submission = await unitOfWork.Submissions
                .Query()
                .Where(s =>
                    s.AssignmentId == request.AssignmentId &&
                    s.StudentId == studentId &&
                    !s.IsDeleted)
                .Select(s => new MySubmissionDto
                {
                    Id = s.Id,
                    AssignmentId = s.AssignmentId,
                    AssignmentTitle = s.Assignment.Title,
                    MaxScore = s.Assignment.MaxScore,
                    DueDate = s.Assignment.DueDate,
                    FileName = s.FileName,
                    FileUrl = $"{_httpContextAccessor.HttpContext!.Request.Scheme}" +
                $"://{_httpContextAccessor.HttpContext!.Request.Host}" +
                $"/{s.FileUrl}",
                    ContentType = s.ContentType,
                    FileSize = s.FileSize,
                    Comment = s.Comment,
                    Score = s.Score,
                    Feedback = s.Feedback,
                    IsLate = s.IsLate,
                    Status = s.Status.ToString(),
                    SubmittedAt = s.SubmittedAt,
                    GradedAt = s.GradedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (submission is null)
            {
                return Result<MySubmissionDto>.Failure(
                    ResultStatus.NotFound,
                    "You have not submitted this assignment yet.");
            }

            return Result<MySubmissionDto>.Success(submission);
        }
    }
}
