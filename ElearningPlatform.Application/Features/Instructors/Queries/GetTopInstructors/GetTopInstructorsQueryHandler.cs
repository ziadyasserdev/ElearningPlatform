using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Instructors.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetTopInstructors
{
    public class GetTopInstructorsQueryHandler : IRequestHandler<GetTopInstructorsQuery, Result<List<InstructorDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetTopInstructorsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<InstructorDto>>> Handle(GetTopInstructorsQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Instructors.Query()
                 .AsNoTracking()
                 .Where(x => x.User.IsActive && !x.User.IsDeleted && x.User.IsInstructor);
            var topInstructors= await query.OrderByDescending(x => x.Rating)
                .ThenByDescending(x => x.TotalStudents)
                .Take(request.Count)
                .Select(x => new InstructorDto
                {
                    Id = x.Id,
                    FullName = x.User.FullName,
                    ProfileImageUrl = x.User.ProfileImageUrl,
                    Bio = x.Bio,
                    ExperienceYears = x.ExperienceYears,
                    Rating = x.Rating,
                    Specialization = x.Specialization,
                })
                .ToListAsync(cancellationToken);

            return Result<List<InstructorDto>>.Success(topInstructors);

        }
    }
}
