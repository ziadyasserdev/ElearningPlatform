using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Queries.CheckIfUserIsInstructor
{
    public class CheckIfUserIsInstructorQueryHandler : IRequestHandler<CheckIfUserIsInstructorQuery, Result<bool>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICurrentUserService currentUserService;

        public CheckIfUserIsInstructorQueryHandler(UserManager<ApplicationUser> userManager,ICurrentUserService currentUserService)
        {
            this.userManager = userManager;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<bool>> Handle(CheckIfUserIsInstructorQuery request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated || string.IsNullOrWhiteSpace(currentUserService.UserId))
                return Result<bool>.Failure(ResultStatus.Unauthorized, "User must be authenticated.");

            var userId = currentUserService.UserId;

            var user = await userManager.Users
                .Where(u => u.Id == userId && !u.IsDeleted)
                .Select(u => new { u.IsInstructor })
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
                return Result<bool>.Failure(ResultStatus.NotFound, "User not found.");

            return Result<bool>.Success(user.IsInstructor);

        }
    }
}
