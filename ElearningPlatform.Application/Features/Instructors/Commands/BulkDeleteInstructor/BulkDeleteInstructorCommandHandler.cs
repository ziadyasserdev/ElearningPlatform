using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
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

namespace ElearningPlatform.Application.Features.Instructors.Commands.BulkDeleteInstructor
{
    public class BulkDeleteInstructorCommandHandler : IRequestHandler<BulkDeleteInstructorCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public BulkDeleteInstructorCommandHandler(UserManager<ApplicationUser> userManager,IUnitOfWork unitOfWork,ICurrentUserService currentUserService)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(BulkDeleteInstructorCommand request, CancellationToken cancellationToken)
        {
            var instructorsId = request.InstructorIds.Select(id => id.ToString()).ToList();
            var instructors = await userManager.Users
                .Include(x => x.InstructorProfile)
                .Where(u => instructorsId.Contains(u.Id) && u.IsInstructor)
                .ToListAsync(cancellationToken);
            var deletedCount = 0;
            foreach(var instructor in instructors)
            {
                if (instructor.IsDeleted)
                    continue;
                if (instructor.Courses.Any())
                    return Result<string>.Failure(ResultStatus.Failure, $"Instructor with ID {instructor.Id} cannot be deleted because they have associated courses");
                deletedCount++;
                instructor.DeletedAt = DateTime.UtcNow;
                instructor.DeletedBy = currentUserService.UserName;
                instructor.IsDeleted = true;
                instructor.IsActive = false;
                instructor.InstructorProfile = null;
                var deleteResult = await userManager.UpdateAsync(instructor);
                if (!deleteResult.Succeeded)
                    return Result<string>.Failure(ResultStatus.Failure, $"Failed to delete instructor ID: {instructor.Id}");
              
            }
            return Result<string>.Success($"{deletedCount} instructor(s) have been deleted successfully.");

        }
    }
}
