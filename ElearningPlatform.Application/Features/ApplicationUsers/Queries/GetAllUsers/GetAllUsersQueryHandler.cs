using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Features.ApplicationUsers.Dtos;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.ApplicationUsers.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<List<UserDto>>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUnitOfWork unitOfWork;

        public GetAllUsersQueryHandler(UserManager<ApplicationUser> userManager,IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<List<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await userManager.Users.Select(x => new
            UserDto
            {
                Id = x.Id,
                Email = x.Email!,
                FullName = x.FullName,
                Gender = x.Gender.ToString(),
                PhoneNumber = x.PhoneNumber,
                UserName = x.UserName!,
              

            }).ToListAsync(cancellationToken);
            return Result<List<UserDto>>.Success(users);
        }
    }
}
