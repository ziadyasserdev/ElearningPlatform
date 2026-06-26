using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.ApplicationUsers.Commands.DeleteUserByAdmin
{
    public class DeleteUserByAdminCommandHandler : IRequestHandler<DeleteUserByAdminCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public DeleteUserByAdminCommandHandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Result<string>> Handle(DeleteUserByAdminCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user is null)
                return Result<string>.Failure(ResultStatus.NotFound, $"User with id {request.UserId} not found");
            user.IsDeleted = true;
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errorMessage = string.Join(" | ", result.Errors.Select(e => e.Description));

                return Result<string>.Failure(
                    ResultStatus.Failure,
                    $"Delete User failed. Details: {errorMessage}"
                );
            }

            return Result<string>.Success(user.Id);
        }
    }
}
