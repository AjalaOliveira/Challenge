using Challenge.Business.Interfaces;
using Challenge.Business.Notifications;
using Challenge.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Challenge.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<IPeopleService, PeopleService>();
            services.AddScoped<INotificator, Notificator>();
            return services;
        }
    }
}