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

namespace ElearningPlatform.Application.Features.Instructors.Queries.SearchInstructors
{
    public class SearchInstructorsQueryHandler : IRequestHandler<SearchInstructorsQuery, Result<List<InstructorDto>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public SearchInstructorsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<InstructorDto>>> Handle(SearchInstructorsQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Instructors.Query()
                 .AsNoTracking()
                 .Where(x => !x.User.IsDeleted && x.User.IsActive && x.User.IsInstructor);

          
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                query = query.Where(x => x.User.FullName.Contains(request.Name));
            }

          
            if (!string.IsNullOrWhiteSpace(request.Specialization))
            {
                query = query.Where(x => x.Specialization.Contains(request.Specialization));
            }

          
            if (request.MinRating.HasValue)
            {
                query = query.Where(x => x.Rating >= request.MinRating.Value);
            }

            var result = await query
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

            if(!result.Any())
                return Result<List<InstructorDto>>.Failure(ResultStatus.NotFound, "No instructors found matching the criteria.");

            return Result<List<InstructorDto>>.Success(result);

           
 
        }
    }
}
