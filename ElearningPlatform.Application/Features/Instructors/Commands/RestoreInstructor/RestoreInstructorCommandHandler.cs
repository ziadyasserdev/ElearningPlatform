using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Commands.RestoreInstructor
{
    public class RestoreInstructorCommandHandler : IRequestHandler<RestoreInstructorCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public RestoreInstructorCommandHandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Result<string>> Handle(RestoreInstructorCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.UserId);

            if (user is null || !user.IsInstructor)
                return Result<string>.Failure(ResultStatus.NotFound, "Instructor not found");

            if (!user.IsDeleted)
                return Result<string>.Failure(ResultStatus.Failure, "Instructor is not deleted");

            user.IsDeleted = false;
            user.DeletedBy = null;
            user.DeletedAt = null;
            user.UpdatedAt = DateTime.UtcNow;

            var updateResult = await userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
                return Result<string>.Failure(ResultStatus.Failure, "Failed to restore instructor");

            return Result<string>.Success("Instructor restored successfully");
        }
    }
}
