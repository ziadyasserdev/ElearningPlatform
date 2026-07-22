using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Enrollments.Queries.AdminGetAllEnrollments
{
    public class GetAllEnrollmentsQuery
       : IRequest<Result<List<AdminEnrollmentDtoo>>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public int? CourseId { get; set; }

        public EnrollmentStatus? Status { get; set; }

        public string? Search { get; set; }
    }
    public class AdminEnrollmentDtoo
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public string CourseTitle { get; set; }

        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }

        public decimal ProgressPercentage { get; set; }

        public string Status { get; set; }

        public DateTime EnrolledAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
