using ElearningPlatform.Application.Contracts.ExternalServices;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Payments;
using ElearningPlatform.Application.Contracts.Repositories;
using ElearningPlatform.Application.Contracts.Services;
using ElearningPlatform.Domain.Identity;
using ElearningPlatform.Infrastructure.ExternalServices;
using ElearningPlatform.Infrastructure.Identity;
using ElearningPlatform.Infrastructure.Payments;
using ElearningPlatform.Infrastructure.Persistence.Context;
using ElearningPlatform.Infrastructure.Repositories;
using ElearningPlatform.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElearningPlatform.Infrastructure.Extensions
{

    public static class InfrastructureDependencyRegisteration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
          
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DbConn")));


            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
           ;
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            // تسجيل IHttpContextAccessor
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPaymentService, StripePaymentService>();
            services.Configure<StripeOptions>(
    configuration.GetSection(StripeOptions.SectionName));
            services.AddScoped<IStripeWebhookService, StripeWebhookService>();
            services.Configure<StripeOptions>(
    configuration.GetSection("Stripe"));

            services.AddScoped<IPaymentService, StripePaymentService>();

            services.AddScoped<IStripeWebhookService, StripeWebhookService>();

            services.AddScoped<IVideoProcessingService, VideoProcessingService>();
            return services;
        }
    }

}