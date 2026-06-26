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

namespace ElearningPlatform.Application.Features.Authentications.Commands.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<AuthResponseDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthService authService;

        public RegisterUserCommandHandler(UserManager<ApplicationUser> userManager,IAuthService authService)
        {
            this._userManager = userManager;
            this.authService = authService;
        }
        public async Task<Result<AuthResponseDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                return Result<AuthResponseDto>.Failure(ResultStatus.Failure, "Email already exists.");
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName,
                Gender = request.Gender,
                PhoneNumber = request.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<AuthResponseDto>.Failure(ResultStatus.Failure, errors);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "Student");

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return Result<AuthResponseDto>.Failure(ResultStatus.Failure, "Failed to assign role.");
            }

            var token = await authService.GenerateToken(user);

            var authResponse = new AuthResponseDto
            {
                Auth = token,
                User = new UserResponseDto
                {
                    Email = user.Email,
                    Id = user.Id,
                    Name = user.FullName,
                    Role = "Student"
                }
            };

            return Result<AuthResponseDto>.Success(authResponse, "Registration successful.");
        }
    }
}
