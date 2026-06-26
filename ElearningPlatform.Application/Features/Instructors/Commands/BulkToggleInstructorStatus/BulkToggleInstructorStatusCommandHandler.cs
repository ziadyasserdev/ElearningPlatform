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

namespace ElearningPlatform.Application.Features.Instructors.Commands.BulkToggleInstructorStatus
{
    public class BulkToggleInstructorStatusCommandHandler : IRequestHandler<BulkToggleInstructorStatusCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public BulkToggleInstructorStatusCommandHandler(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        public async Task<Result<string>> Handle(BulkToggleInstructorStatusCommand request, CancellationToken cancellationToken)
        {
            var instructorIds = request.UserIds.Distinct().ToList();

            var instructors = await unitOfWork.Instructors.Query()
                .Include(x => x.User)
                .Where(x => instructorIds.Contains(x.UserId) && !x.User.IsDeleted && x.User.IsInstructor)
                .ToListAsync(cancellationToken);

            if (!instructors.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "No valid instructors found.");

            int updatedCount = 0;

            foreach (var instructor in instructors)
            {
                if (instructor.User.IsActive == request.IsActive)
                    continue; 

                instructor.User.IsActive = request.IsActive;
                instructor.User.UpdatedAt = DateTime.UtcNow;

                var updateResult = await userManager.UpdateAsync(instructor.User);
                if (!updateResult.Succeeded)
                    return Result<string>.Failure(ResultStatus.Failure, $"Failed to update instructor ID: {instructor.Id}");

                updatedCount++;
            }

            return Result<string>.Success($"{updatedCount} instructor(s) have been {(request.IsActive ? "activated" : "deactivated")} successfully.");

        }
    }
}
