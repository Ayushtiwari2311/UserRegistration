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
                    //if (env.IsDevelopment())
                    //{
                    //    policy
                    //        .AllowAnyOrigin()
                    //        .AllowAnyHeader()
                    //        .AllowAnyMethod()
                    //        .AllowCredentials();
                    //}
                    //else
                    //{
                        policy
                            .WithOrigins("http://localhost:5173") // ✅ Exact origin of your React app
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    //}
                });
            });

            return services;
        }

        public static string GetPolicyName() => PolicyName;
    }
}
