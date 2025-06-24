using API.Helpers.Image;
namespace API.DependencyInjection
{
	public static class ServiceContainer
	{
		public static IServiceCollection AddAPIService
			(this IServiceCollection services)
		{
			services.AddScoped<IImageHelper, ImageHelper>();
			return services;
		}
	}
}
