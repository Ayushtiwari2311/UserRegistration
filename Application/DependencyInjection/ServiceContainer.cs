using Application.UseCaseImplementation;
using Application.UseCaseInterfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationService
            (this IServiceCollection services)
        {
            services.AddScoped<IUserRegistrationService, UserRegistrationService>();
            services.AddScoped<IMastersService, MastersService>();
            return services;
        }
    }
    
}
