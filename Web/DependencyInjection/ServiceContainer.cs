using MVC.Services.Auth;
using MVC.Services.Master;
using MVC.Services.UserRegistration;
namespace MVC.DependencyInjection
{
	public static class ServiceContainer
	{
		public static IServiceCollection AddWebService
			(this IServiceCollection services)
		{
			services.AddScoped<IUserRegistrationService, UserRegistrationService>();
			services.AddScoped<IMasterService, MasterService>();
			services.AddScoped<IAuthService, AuthService>();
			return services;
		}
	}
}
