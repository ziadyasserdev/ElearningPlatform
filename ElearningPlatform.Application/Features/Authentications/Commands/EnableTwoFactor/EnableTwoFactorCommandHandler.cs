using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authentications.Commands.EnableTwoFactor
{
    public class EnableTwoFactorCommandHandler : IRequestHandler<EnableTwoFactorCommand, Result<string>>
    {

        private readonly UserManager<ApplicationUser> userManager;

        public EnableTwoFactorCommandHandler(UserManager<ApplicationUser> userManager)
        {

            this.userManager = userManager;
        }
        public async Task<Result<string>> Handle(EnableTwoFactorCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.UserId);

            if (user == null)
                return Result<string>.Failure(ResultStatus.NotFound, "User not found");
            user.TwoFactorEnabled = true;

            await userManager.UpdateAsync(user);

            return Result<string>.Success("Two factor enabled successfully");
        }
    }
}
