using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.Services;
using ElearningPlatform.Application.Features.Authentications.Dtos;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authentications.Commands.VerifyTwoFactor
{
    public class VerifyTwoFactorCommandHandler : IRequestHandler<VerifyTwoFactorCommand, Result<AuthTokenDto>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAuthService authService;

        public VerifyTwoFactorCommandHandler(UserManager<ApplicationUser> userManager, IAuthService authService)
        {
            this.userManager = userManager;
            this.authService = authService;
        }
        public async Task<Result<AuthTokenDto>> Handle(VerifyTwoFactorCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return Result<AuthTokenDto>.Failure(
                    ResultStatus.NotFound,
                    "User not found");

            var isValid = await userManager.VerifyTwoFactorTokenAsync(
                user,
                TokenOptions.DefaultEmailProvider,
                request.Code);

            if (!isValid)
                return Result<AuthTokenDto>.Failure(
                    ResultStatus.Failure,
                    "Invalid verification code");

            var token = await authService.GenerateToken(user);

            return Result<AuthTokenDto>.Success(token);
        }
    }
}
