using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authentications.Commands.UnlockUser
{
    public class UnlockUserCommandHandler : IRequestHandler<UnlockUserCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UnlockUserCommandHandler(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Result<string>> Handle(UnlockUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.UserId);
            if (user == null)
                return Result<string>.Failure(ResultStatus.NotFound, "User not found");
            if (!user.IsLocked)
                return Result<string>.Failure(ResultStatus.Conflict, "User is not locked");
            user.IsLocked = false;
            user.LockedAt = null;
            user.LockedByAdminId = null;
            await userManager.UpdateAsync(user);
            return Result<string>.Success("User account has been unlocked successfully");
        }
    }
}
