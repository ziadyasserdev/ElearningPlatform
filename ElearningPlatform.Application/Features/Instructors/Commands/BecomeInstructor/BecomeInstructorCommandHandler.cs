using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Identity;
using ElearningPlatform.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Commands.BecomeInstructor
{
    public class BecomeInstructorCommandHandler : IRequestHandler<BecomeInstructorCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICurrentUserService currentUserService;
        private readonly IUnitOfWork unitOfWork;

        public BecomeInstructorCommandHandler(UserManager<ApplicationUser> userManager
            ,ICurrentUserService currentUserService
            ,IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.currentUserService = currentUserService;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(BecomeInstructorCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated || string.IsNullOrWhiteSpace(currentUserService.UserId))
                return Result<string>.Failure(ResultStatus.Unauthorized, "User must be authenticated.");

            var userId = currentUserService.UserId;

            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
                return Result<string>.Failure(ResultStatus.NotFound, "User not found.");

            if (user.IsInstructor)
                return Result<string>.Failure(ResultStatus.Conflict, "User is already an instructor.");

           
            user.IsInstructor = true;
            user.UpdatedAt = DateTime.UtcNow;

            var updateResult = await userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
                return Result<string>.Failure(ResultStatus.Failure, errors);
            }

         
            var instructor = new InstructorProfile
            {
                UserId = user.Id,
                Bio = request.Bio,
                Specialization = request.Specialization,
                ExperienceYears = request.ExperienceYears,
                LinkedInUrl = request.LinkedInUrl ?? string.Empty,
                WebsiteUrl = request.WebsiteUrl ?? string.Empty,
                Rating = 0,
                TotalCourses = 0,
                TotalStudents = 0
            };

            await unitOfWork.Instructors.AddAsync(instructor);
            await unitOfWork.SaveAsync();

            return Result<string>.Success("User has successfully become an instructor.");
        }
    }
}
