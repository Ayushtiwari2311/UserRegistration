namespace Presentation.Configurations
{
    public static class CorsConfig
    {
        private const string PolicyName = "AppCorsPolicy";

        public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IHostEnvironment env)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: PolicyName, policy =>
                {
                    if (env.IsDevelopment())
                    {
                        policy
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    }
                    else
                    {
                        policy
                            .WithOrigins("https://localhost:7059") 
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    }
                });
            });

            return services;
        }

        public static string GetPolicyName() => PolicyName;
    }
}
