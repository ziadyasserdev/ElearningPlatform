using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Domain.Identity;
using ElearningPlatform.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Instructors.Commands.AddInstructor
{
    public class AddInstructorCommandHandler : IRequestHandler<AddInstructorCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AddInstructorCommandHandler(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        
        public async Task<Result<string>> Handle(AddInstructorCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return Result<string>.Failure(ResultStatus.Failure, "User with this email already exists.");

            var user = new ApplicationUser
            {
                FullName = request.FullName,
                Email = request.Email,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber,
                IsActive = true,
                IsInstructor = true
            };

            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return Result<string>.Failure(ResultStatus.Failure, string.Join(", ", result.Errors.Select(e => e.Description)));

            if (!await roleManager.RoleExistsAsync("Instructor"))
                await roleManager.CreateAsync(new IdentityRole("Instructor"));
            await userManager.AddToRoleAsync(user, "Instructor");

            var InstructorProfile = new InstructorProfile
            {
                Bio = request.Bio,
                UserId = user.Id,
                Specialization = request.Specialization,
                ExperienceYears = request.ExperienceYears,
                LinkedInUrl = request.LinkedInUrl,
                WebsiteUrl = request.WebsiteUrl,
                Rating = 0,
                TotalCourses = 0,
                TotalStudents = 0
            };

            await unitOfWork.Instructors.AddAsync(InstructorProfile);
            await unitOfWork.SaveAsync();   
            
            return Result<string>.Success(user.Id);
        }
    }
}
