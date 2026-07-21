using ElearningPlatform.Api.Extensions;
using ElearningPlatform.Api.Middleware;
using ElearningPlatform.Application.Contracts.Identity;
using ElearningPlatform.Application.Contracts.Payments;
using ElearningPlatform.Application.Extensions;
using ElearningPlatform.Application.Settings;
using ElearningPlatform.Domain.Identity;
using ElearningPlatform.Infrastructure.Extensions;
using ElearningPlatform.Infrastructure.Identity;
using ElearningPlatform.Infrastructure.Payments;
using ElearningPlatform.Infrastructure.Persistence.Context;
using ElearningPlatform.Infrastructure.Persistence.SeedData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json.Serialization;

namespace ElearningPlatform.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });


         
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
            });

   

          


            builder.Services.AddInfrastructureServices(builder.Configuration)
                .AddApplicationDependency()
                .AddApiServices();
            builder.Services.Configure<JwtSetting>(
           builder.Configuration.GetSection("JwtSetting")
       );
            builder.Services.Configure<FileStorageSettings>(builder.Configuration.GetSection("FileStorageSettings"));

            // ????? IHttpContextAccessor
            builder.Services.AddHttpContextAccessor();




            builder.Services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateIssuerSigningKey = true,

                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["JwtSetting:Issuer"],
                    ValidAudience = builder.Configuration["JwtSetting:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.
                    Configuration["JwtSetting:SecretKey"]))
                };
            });



            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

            builder.Services.Configure<EmailSettings>(
              builder.Configuration.GetSection("EmailSettings")
          );



            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme,
    securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization : `Bearer Genreated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = JwtBearerDefaults.AuthenticationScheme
            }
        },
        new string[] { }
    }
});

            });





            var app = builder.Build();


           
            // Configure the HTTP request pipeline.
          
                app.UseSwagger();
                app.UseSwaggerUI();
            
            app.UseCors("AllowAll");

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
           

            app.MapControllers();
           app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
            app.Run();
        }
    }
}
