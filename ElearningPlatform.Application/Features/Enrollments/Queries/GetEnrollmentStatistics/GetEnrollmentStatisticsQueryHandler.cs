using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Enrollments.Dtos;
using ElearningPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Queries.GetEnrollmentStatistics
{

    public class GetEnrollmentStatisticsQueryHandler
          : IRequestHandler<GetEnrollmentStatisticsQuery,
              Result<EnrollmentStatisticsDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetEnrollmentStatisticsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<EnrollmentStatisticsDto>> Handle(
            GetEnrollmentStatisticsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<EnrollmentStatisticsDto>.Failure(
                    ResultStatus.Unauthorized,
                    "Authentication required.");
            }

            var query = unitOfWork.Enrollments.Query()
                .AsNoTracking();

            if (currentUserService.IsInRole("Instructor"))
            {
                var instructorId = currentUserService.UserId;

                query = query.Where(x =>
                    x.Course.Instructor.UserId == instructorId);
            }
            else if (!currentUserService.IsInRole("Admin"))
            {
                return Result<EnrollmentStatisticsDto>.Failure(
                    ResultStatus.Forbidden,
                    "Access denied.");
            }

            var total = await query.CountAsync(cancellationToken);

            var active = await query.CountAsync(
                x => x.Status == EnrollmentStatus.Active,
                cancellationToken);

            var completed = await query.CountAsync(
                x => x.Status == EnrollmentStatus.Completed,
                cancellationToken);

            var cancelled = await query.CountAsync(
                x => x.Status == EnrollmentStatus.Cancelled,
                cancellationToken);

            decimal completionRate = 0;

            if (total > 0)
            {
                completionRate =
                    Math.Round((decimal)completed / total * 100, 2);
            }

            return Result<EnrollmentStatisticsDto>.Success(
                new EnrollmentStatisticsDto
                {
                    TotalEnrollments = total,
                    ActiveEnrollments = active,
                    CompletedEnrollments = completed,
                    CancelledEnrollments = cancelled,
                    CompletionRate = completionRate
                });
        }
    }
}
