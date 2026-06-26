using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Queries.AdminGetAllEnrollments
{

    public class GetAllEnrollmentsQueryHandler
        : IRequestHandler<GetAllEnrollmentsQuery, Result<List<AdminEnrollmentDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetAllEnrollmentsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<AdminEnrollmentDto>>> Handle(
            GetAllEnrollmentsQuery request,
            CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<List<AdminEnrollmentDto>>.Failure(ResultStatus.Unauthorized, "Authentication required");
            var query = unitOfWork.Enrollments.Query()
                .Include(x => x.Course)
                    .ThenInclude(x => x.Instructor)
                .Include(x => x.Student)
                .AsQueryable();

            if (currentUserService.IsInRole("Instructor"))
            {
                var userId = currentUserService.UserId;

                query = query.Where(x =>
                    x.Course.Instructor.UserId == userId);
            }
            else if (!currentUserService.IsInRole("Admin"))
            {
                return Result<List<AdminEnrollmentDto>>
                    .Failure(ResultStatus.Forbidden, "Not allowed");
            }

            if (request.CourseId.HasValue)
            {
                query = query.Where(x =>
                    x.CourseId == request.CourseId.Value);
            }

            if (request.Status.HasValue)
            {
                query = query.Where(x =>
                    x.Status == request.Status.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x =>
                    x.Student.FullName.Contains(request.Search) ||
                    x.Student.Email.Contains(request.Search));
            }

            var enrollments = await query
                .OrderByDescending(x => x.EnrolledAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new AdminEnrollmentDto
                {
                    Id = x.Id,

                    CourseId = x.CourseId,
                    CourseTitle = x.Course.Title,

                    StudentId = x.StudentId,
                    StudentName = x.Student.FullName,
                    StudentEmail = x.Student.Email,

                    ProgressPercentage = x.ProgressPercentage,

                    Status = x.Status.ToString(),

                    EnrolledAt = x.EnrolledAt,

                    CompletedAt = x.CompletedAt
                })
                .ToListAsync(cancellationToken);

            return Result<List<AdminEnrollmentDto>>
                .Success(enrollments);
        }
    }
}
