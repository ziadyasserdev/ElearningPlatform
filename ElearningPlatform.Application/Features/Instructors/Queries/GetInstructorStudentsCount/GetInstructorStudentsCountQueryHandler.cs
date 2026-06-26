using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetInstructorStudentsCount
{
    public class GetInstructorStudentsCountQueryHandler : IRequestHandler<GetInstructorStudentsCountQuery, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetInstructorStudentsCountQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(GetInstructorStudentsCountQuery request, CancellationToken cancellationToken)
        {
            var instructor = await unitOfWork.Instructors.GetByIdAsync(request.InstructorId);

            if (instructor is null)
                return Result<int>.Failure(ResultStatus.NotFound, "Instructor not found.");

            var studentsCount = await unitOfWork.Courses.Query()
                .Where(c => c.InstructorId == request.InstructorId)
                .SelectMany(c => c.Enrollments)
                .Select(e => e.StudentId)
                .Distinct()
                .CountAsync(cancellationToken);

            return Result<int>.Success(studentsCount);

        }
    }
}
