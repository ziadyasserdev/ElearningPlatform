using ElearningPlatform.Api.Filters;
using Microsoft.OpenApi.Models;

namespace ElearningPlatform.Api.Extensions
{
    public static class ApiDependencyRegisteration
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "E-Learning Platform API",
                    Version = "v1"
                });

              
                options.DocumentFilter<SortPathsDocumentFilter>();
            });

            return services;
        }
    }
}
