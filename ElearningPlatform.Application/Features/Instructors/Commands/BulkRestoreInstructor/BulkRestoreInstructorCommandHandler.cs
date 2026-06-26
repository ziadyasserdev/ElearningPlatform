using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Commands.BulkRestoreInstructor
{
    public class BulkRestoreInstructorCommandHandler : IRequestHandler<BulkRestoreInstructorCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUnitOfWork unitOfWork;

        public BulkRestoreInstructorCommandHandler(UserManager<ApplicationUser> userManager,IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(BulkRestoreInstructorCommand request, CancellationToken cancellationToken)
        {
            var instructorIds = request.InstructorIds
       .Select(id => id.ToString())
       .Distinct()
       .ToList();

            var instructors = await userManager.Users
                .Where(x => instructorIds.Contains(x.Id)
                         && x.IsInstructor
                         && x.IsDeleted)
                .ToListAsync(cancellationToken);

            if (!instructors.Any())
                return Result<string>.Failure(ResultStatus.NotFound, "No deleted instructors found to restore");

            var restoredCount = 0;

            foreach (var instructor in instructors)
            {
                instructor.IsDeleted = false;
                instructor.DeletedBy = null;
                instructor.DeletedAt = null;
                instructor.UpdatedAt = DateTime.UtcNow;

                var restoreResult = await userManager.UpdateAsync(instructor);

                if (!restoreResult.Succeeded)
                    return Result<string>.Failure(
                        ResultStatus.Failure,
                        $"Failed to restore instructor ID: {instructor.Id}"
                    );

                restoredCount++;
            }

            return Result<string>.Success($"{restoredCount} instructor(s) have been restored successfully.");
        }
    }
}
