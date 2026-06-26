using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.Instructors.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.GetAllInstructors
{
    public class GetAllInstructorsQueryHandler : IRequestHandler<GetAllInstructorsQuery, Result<List<InstructorDto>>>
    {
        private readonly IUnitOfWork unitOfWork;
      

        public GetAllInstructorsQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
           
        }
        public async Task<Result<List<InstructorDto>>> Handle(GetAllInstructorsQuery request, CancellationToken cancellationToken)
        {
            var instructors = await unitOfWork.Instructors.Query()
       .AsNoTracking()
       .Where(x => !x.User.IsDeleted && x.User.IsInstructor && x.User.IsActive)
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

            return Result<List<InstructorDto>>.Success(instructors);

        }
    }
}
