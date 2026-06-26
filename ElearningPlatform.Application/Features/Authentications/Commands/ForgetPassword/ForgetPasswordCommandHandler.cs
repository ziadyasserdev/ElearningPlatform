using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.ExternalServices;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authentications.Commands.ForgetPassword
{
    public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailService emailService;

        public ForgetPasswordCommandHandler(UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            this.userManager = userManager;
            this.emailService = emailService;
        }
        public async Task<Result<string>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Result<string>.Success("If email exists, reset link sent");
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(token));
            var resetLink =
           $"http://localhost:5118/reset-password?email={user.Email}&token={encodedToken}";
            await emailService.SendEmailAsync(request.Email, "Reset Password", $"Click here: {resetLink}");
            return Result<string>.Success("If email exists, reset link sent");
        }
    }

}
