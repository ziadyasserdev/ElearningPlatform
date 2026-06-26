using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Commands.ToggleInstructorStatus
{
    public class ToggleInstructorStatusCommandHandler : IRequestHandler<ToggleInstructorStatusCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ToggleInstructorStatusCommandHandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Result<string>> Handle(ToggleInstructorStatusCommand request, CancellationToken cancellationToken)
        {
           var user = await userManager.FindByIdAsync(request.UserId);
            if (user is null || !user.IsInstructor)
                return Result<string>.Failure(ResultStatus.NotFound, "Instructor not found");
            if (user.IsDeleted)
                    return Result<string>.Failure(ResultStatus.Failure, "Cannot change status of a deleted instructor");
            if (request.IsActive && user.IsActive)
                return Result<string>.Failure(ResultStatus.Failure, "Instructor is already active");
            if (!request.IsActive && !user.IsActive)
                return Result<string>.Failure(ResultStatus.Failure, "Instructor is already inactive");
            user.IsActive = request.IsActive;
            user.UpdatedAt = DateTime.UtcNow;
            var updateResult = await userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
               return Result<string>.Failure(ResultStatus.Failure, "Failed to update instructor status");

            return Result<string>.Success(request.IsActive == true ? "Instructor activated successfully" : "Instructor deactivated successfully");
        }
    }
}
