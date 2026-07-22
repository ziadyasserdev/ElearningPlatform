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

namespace ElearningPlatform.Application.Features.Enrollments.Queries.GetEnrollmentAnalytics
{
    public class GetEnrollmentAnalyticsQueryHandler
      : IRequestHandler<GetEnrollmentAnalyticsQuery,
          Result<List<EnrollmentAnalyticsDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetEnrollmentAnalyticsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<Result<List<EnrollmentAnalyticsDto>>> Handle(
            GetEnrollmentAnalyticsQuery request,
            CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
            {
                return Result<List<EnrollmentAnalyticsDto>>
                    .Failure(ResultStatus.Unauthorized,
                        "Authentication required.");
            }

            IQueryable<Domain.Models.Enrollment> query = unitOfWork.Enrollments.Query()
                .Where(x => x.EnrolledAt.Year == request.Year);

            if (currentUserService.IsInRole("Instructor"))
            {
                var userId = currentUserService.UserId;

                query = query.Where(x =>
                    x.Course.Instructor.UserId == userId);
            }
            else if (!currentUserService.IsInRole("Admin"))
            {
                return Result<List<EnrollmentAnalyticsDto>>
                    .Failure(ResultStatus.Forbidden,
                        "Access denied.");
            }

            var analytics = await query
                .GroupBy(x => x.EnrolledAt.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Count = g.Count()
                })
                .ToListAsync(cancellationToken);

            var result = Enumerable.Range(1, 12)
                .Select(month => new EnrollmentAnalyticsDto
                {
                    Month = new DateTime(request.Year, month, 1)
                        .ToString("MMMM"),

                    TotalEnrollments = analytics
                        .FirstOrDefault(x => x.Month == month)?.Count ?? 0
                })
                .ToList();

            return Result<List<EnrollmentAnalyticsDto>>
                .Success(result);
        }
    }
}
