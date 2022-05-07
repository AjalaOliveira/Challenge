using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace Challenge.API.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfig(this IServiceCollection services)
        {
            services.AddApiVersioning(avo =>
            {
                avo.UseApiBehavior = false;
                avo.ReportApiVersions = true;
                avo.AssumeDefaultVersionWhenUnspecified = true;
                avo.DefaultApiVersion = new ApiVersion(1, 0);

                avo.ApiVersionReader = ApiVersionReader.Combine(
                    new HeaderApiVersionReader("x-api-version"),
                    new QueryStringApiVersionReader(),
                    new UrlSegmentApiVersionReader());
            });

            return services;
        }
    }
}
