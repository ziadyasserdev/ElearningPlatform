using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Submissions.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Submissions.Queries.GetSubmissionDetails
{
    public class GetSubmissionDetailsQueryHandler : IRequestHandler<GetSubmissionDetailsQuery, Result<SubmissionDetailsDto>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetSubmissionDetailsQueryHandler(IHttpContextAccessor httpContextAccessor,IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this._httpContextAccessor = httpContextAccessor;
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<SubmissionDetailsDto>> Handle(GetSubmissionDetailsQuery request, CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<SubmissionDetailsDto>.Failure(ResultStatus.Unauthorized,"User is not authenticated");
            var instructorId = currentUserService.UserId;
            var submission = await unitOfWork.Submissions.Query()
                .AsNoTracking()
                .Where(s => s.Id == request.SubmissionId && !s.IsDeleted
                && s.Assignment.Course.Instructor.UserId == instructorId)
                .Select(c => new SubmissionDetailsDto
                {
                    Id = c.Id,
                    StudentId = c.StudentId,
                    StudentName = c.Student.FullName,
                    StudentEmail = c.Student.Email,
                    CourseId = c.Assignment.CourseId,
                    CourseTitle = c.Assignment.Course.Title,
                    AssignmentId = c.AssignmentId,
                    AssignmentTitle = c.Assignment.Title,
                    MaxScore = c.Assignment.MaxScore,
                    FileName = c.FileName,
                    FileUrl = $"{_httpContextAccessor.HttpContext!.Request.Scheme}" +
                $"://{_httpContextAccessor.HttpContext!.Request.Host}" +
                $"/{c.FileUrl}",
                    FileSize = c.FileSize,
                    ContentType = c.ContentType,
                    Comment = c.Comment,
                    SubmittedAt = c.SubmittedAt,
                    GradedAt = c.GradedAt,
                    IsLate = c.IsLate,
                    Score = c.Score,
                    Feedback = c.Feedback,
                    Status = c.Status.ToString(),

                }).FirstOrDefaultAsync(cancellationToken);

            if(submission == null)
            {
                return Result<SubmissionDetailsDto>.Failure(ResultStatus.NotFound, "Submission not found");
            }
            return Result<SubmissionDetailsDto>.Success(submission);
        }
    }
}
