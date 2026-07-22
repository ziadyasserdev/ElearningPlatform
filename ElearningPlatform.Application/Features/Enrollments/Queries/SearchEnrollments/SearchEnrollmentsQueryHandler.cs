using ElearningPlatform.Application.Common.PaginatedResults;
using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Enrollments.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Queries.SearchEnrollments
{
    public class SearchEnrollmentsQueryHandler
        : IRequestHandler<SearchEnrollmentsQuery,
            Result<PaginatedResult<AdminEnrollmentDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public SearchEnrollmentsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<PaginatedResult<AdminEnrollmentDto>>> Handle(
            SearchEnrollmentsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<PaginatedResult<AdminEnrollmentDto>>
                    .Failure(ResultStatus.Unauthorized,
                        "Authentication required.");
            }

            IQueryable<Domain.Models.Enrollment> query = unitOfWork.Enrollments.Query()
                .AsNoTracking()
                .Include(x => x.Student)
                .Include(x => x.Course)
                    .ThenInclude(x => x.Instructor)
                        .ThenInclude(x => x.User);

            if (currentUserService.IsInRole("Instructor"))
            {
                query = query.Where(x =>
                    x.Course.Instructor.UserId == currentUserService.UserId);
            }
            else if (!currentUserService.IsInRole("Admin"))
            {
                return Result<PaginatedResult<AdminEnrollmentDto>>
                    .Failure(ResultStatus.Forbidden,
                        "Access denied.");
            }

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x =>
                    x.Student.FullName.Contains(request.Search) ||
                    x.Student.Email.Contains(request.Search) ||
                    x.Course.Title.Contains(request.Search) ||
                    x.Course.Instructor.User.FullName.Contains(request.Search));
            }

            if (request.Status.HasValue)
            {
                query = query.Where(x =>
                    x.Status == request.Status.Value);
            }

            if (request.From.HasValue)
            {
                query = query.Where(x =>
                    x.EnrolledAt >= request.From.Value);
            }

            if (request.To.HasValue)
            {
                query = query.Where(x =>
                    x.EnrolledAt <= request.To.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(x => x.EnrolledAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new AdminEnrollmentDto
                {
                    Id = x.Id,

                    CourseId = x.CourseId,
                    CourseTitle = x.Course.Title,
                    InstructorName = x.Course.Instructor.User.FullName,
                    StudentId = x.StudentId,
                    StudentName = x.Student.FullName,
                    StudentEmail = x.Student.Email!,

                    ProgressPercentage = x.ProgressPercentage,

                    Status = x.Status.ToString(),

                    EnrolledAt = x.EnrolledAt,

                    CompletedAt = x.CompletedAt
                })
                .ToListAsync(cancellationToken);

            return Result<PaginatedResult<AdminEnrollmentDto>>
                .Success(new PaginatedResult<AdminEnrollmentDto>(items,
                request.PageNumber,request.PageSize,totalCount));
        }
    }
}
