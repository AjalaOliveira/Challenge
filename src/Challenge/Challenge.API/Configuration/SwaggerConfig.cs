using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Challenge.API.Configuration
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(sgo =>
            {
                sgo.SwaggerDoc("v1.0", new OpenApiInfo { Title = "Challenge.API", Version = "v1.0" });
            });

            return services;
        }
    }
}