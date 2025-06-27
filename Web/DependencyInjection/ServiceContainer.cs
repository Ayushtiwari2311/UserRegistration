using Web.Services.Master;
namespace Web.DependencyInjection
{
	public static class ServiceContainer
	{
		public static IServiceCollection AddWebService
			(this IServiceCollection services)
		{
			services.AddScoped<IMasterService, MasterService>();
			return services;
		}
	}
}
