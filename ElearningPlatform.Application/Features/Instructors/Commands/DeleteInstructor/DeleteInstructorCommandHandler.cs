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

namespace ElearningPlatform.Application.Features.Instructors.Commands.DeleteInstructor
{
    public class DeleteInstructorCommandHandler : IRequestHandler<DeleteInstructorCommand, Result<string>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICurrentUserService currentUserService;

        public DeleteInstructorCommandHandler(IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager,ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.currentUserService = currentUserService;
        }
        public async Task<Result<string>> Handle(DeleteInstructorCommand request, CancellationToken cancellationToken)
        {

            var user = await userManager.Users
           .Include(u => u.Courses)
           .Include(u => u.InstructorProfile)
           .FirstOrDefaultAsync(u => u.Id == request.UserId && !u.IsDeleted, cancellationToken);

            if (user == null)
                return Result<string>.Failure(ResultStatus.NotFound, "Instructor not found");

            if (user.Courses.Any())
                return Result<string>.Failure(ResultStatus.Failure, "Cannot delete instructor with active courses.");

            user.IsDeleted = true;
            user.IsActive = false;
            if (user.InstructorProfile != null)
            {
            
                user.InstructorProfile = null;
            }

            await userManager.UpdateAsync(user);

            return Result<string>.Success("Instructor deleted successfully");
        }
    }
}
