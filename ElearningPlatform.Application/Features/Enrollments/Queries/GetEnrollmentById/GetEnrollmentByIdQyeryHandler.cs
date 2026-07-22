using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Enrollments.Queries.AdminGetAllEnrollments;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Queries.GetEnrollmentById
{
    public class GetEnrollmentByIdQueryHandler
        : IRequestHandler<GetEnrollmentByIdQuery, Result<AdminEnrollmentDtoo>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetEnrollmentByIdQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<AdminEnrollmentDtoo>> Handle(
            GetEnrollmentByIdQuery request,
            CancellationToken cancellationToken)
        {
            if(!currentUserService.IsAuthenticated)
                return Result<AdminEnrollmentDtoo>.Failure(ResultStatus.Unauthorized, "Authentication required");
            var query = unitOfWork.Enrollments.Query()
                .Include(x => x.Course)
                    .ThenInclude(x => x.Instructor)
                .Include(x => x.Student)
                .Where(x => x.Id == request.Id);

            if (currentUserService.IsInRole("Instructor"))
            {
                var userId = currentUserService.UserId;

                query = query.Where(x =>
                    x.Course.Instructor.UserId == userId);
            }
            else if (!currentUserService.IsInRole("Admin"))
            {
                return Result<AdminEnrollmentDtoo>
                    .Failure(ResultStatus.Forbidden, "Not allowed");
            }

            var enrollment = await query
                .Select(x => new AdminEnrollmentDtoo
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
                .FirstOrDefaultAsync(cancellationToken);

            if (enrollment == null)
            {
                return Result<AdminEnrollmentDtoo>
                    .Failure(ResultStatus.NotFound, "Enrollment not found");
            }

            return Result<AdminEnrollmentDtoo>.Success(enrollment);
        }
    }
}
