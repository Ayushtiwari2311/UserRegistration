using Domain.RepositoryInterfaces;
using Infrastructure.DatabaseContext;
using Infrastructure.RepositoryImplementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService
            (this IServiceCollection services, IConfiguration config)
        {

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            services.AddDbContext<LoggingDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("LogDb")));

            services.AddScoped<IUserRegistrationRepository, UserRegistrationRepository>();
            services.AddScoped<IMastersRepository, MastersRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            return services;
        }
    }
}
