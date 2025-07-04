using Application.AutoMapper;
using Application.Helpers.Image;
using Application.Helpers.Patch;
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
            services.AddAutoMapper(cfg => {
                cfg.AddProfile(new AutoMapperProfile());
            });
            services.AddTransient<ImageUrlResolver>();
            services.AddScoped<IUserRegistrationService, UserRegistrationService>();
            services.AddScoped(typeof(IGenericService<,,,>), typeof(GenericService<,,,>));
            services.AddScoped<IMastersService, MastersService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IImageHelper, ImageHelper>();
            services.AddScoped<IPatchHelper, PatchHelper>();
            return services;
        }
    }
    
}
