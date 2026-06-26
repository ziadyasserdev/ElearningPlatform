using ElearningPlatform.Application.Common.Results;
using ElearningPlatform.Application.Contracts.ExternalServices;
using ElearningPlatform.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Application.Features.Authentications.Commands.ResendConfirmation
{
    public class ResendConfirmationCommandHandler : IRequestHandler<ResendConfirmationCommand, Result<string>>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailService emailService;
        private readonly IConfiguration configuration;

        public ResendConfirmationCommandHandler(UserManager<ApplicationUser> userManager
            , IEmailService emailService
            , IConfiguration configuration)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.configuration = configuration;
        }
        public async Task<Result<string>> Handle(ResendConfirmationCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null || user.EmailConfirmed)
                return Result<string>.Success("If the email exists, a confirmation link has been sent.");
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            var encodedToken = WebEncoders.Base64UrlEncode(
                Encoding.UTF8.GetBytes(token));
            var confirmationLink =
           $"{configuration["AppUrl"]}/api/Authentications/confirm-email?userId={user.Id}&token={encodedToken}";

            var body = $"<a href='{confirmationLink}'>Confirm Email</a>";

            await emailService.SendEmailAsync(user.Email!, "Confirm Email", body);

            return Result<string>.Success("If the email exists, a confirmation link has been sent.");
        }
    }
}
