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

namespace ElearningPlatform.Application.Features.Enrollments.Commands.CancelEnrollment
{
    public class CancelEnrollmentCommandHandler
        : IRequestHandler<CancelEnrollmentCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public CancelEnrollmentCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<string>> Handle(
            CancelEnrollmentCommand request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "Authentication required");

            var userId = currentUserService.UserId;

            var enrollment = await unitOfWork.Enrollments.Query()
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e =>
                    e.CourseId == request.CourseId &&
                    e.StudentId == userId,
                    cancellationToken);

            if (enrollment is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Enrollment not found");

            
            if (enrollment.Status == EnrollmentStatus.Cancelled)
                return Result<string>.Failure(ResultStatus.Conflict, "Enrollment already cancelled");

            
            if (enrollment.Status == EnrollmentStatus.Completed)
                return Result<string>.Failure(ResultStatus.Conflict, "Cannot cancel completed course");

           
            var wasActive = enrollment.Status == EnrollmentStatus.Active;

           
            enrollment.Status = EnrollmentStatus.Cancelled;
            enrollment.UpdatedAt = DateTime.Now;
            enrollment.UpdatedBy = currentUserService.UserName;

         
            if (wasActive)
            {
                enrollment.Course.TotalStudents =
                    Math.Max(0, enrollment.Course.TotalStudents - 1);
            }

            await unitOfWork.SaveAsync();

            return Result<string>.Success("Enrollment cancelled successfully");
        }
    }
}
