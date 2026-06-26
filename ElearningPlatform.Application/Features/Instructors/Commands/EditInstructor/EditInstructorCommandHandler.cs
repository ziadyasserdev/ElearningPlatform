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

namespace ElearningPlatform.Application.Features.Instructors.Commands.EditInstructor
{
    public class EditInstructorCommandHandler : IRequestHandler<EditInstructorCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICurrentUserService currentUserService;

        public EditInstructorCommandHandler(IUnitOfWork unitOfWork
            ,UserManager<ApplicationUser> userManager
            ,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(EditInstructorCommand request, CancellationToken cancellationToken)
        {
            if (!currentUserService.IsAuthenticated)
                return Result<string>.Failure(ResultStatus.Unauthorized, "User is not authenticated");

            var userId = currentUserService.UserId;

            var instructor = await unitOfWork.Instructors.Query()
                .FirstOrDefaultAsync(i => i.UserId == userId, cancellationToken);

            if (instructor is null)
                return Result<string>.Failure(ResultStatus.NotFound, "Instructor profile not found");

            instructor.Specialization = request.Specialization?.Trim() ?? instructor.Specialization;
            instructor.ExperienceYears = request.ExperienceYears ?? instructor.ExperienceYears;
            instructor.LinkedInUrl = request.LinkedInUrl ?? instructor.LinkedInUrl;
            instructor.WebsiteUrl = request.WebsiteUrl ?? instructor.WebsiteUrl;

            instructor.Bio = request.Bio ?? instructor.Bio;
            await unitOfWork.SaveAsync();

            return Result<string>.Success("Instructor profile updated successfully");
        }
    }
}
