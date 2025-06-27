using DataTransferObjects.Response.Common;
using System.Threading.RateLimiting;

namespace API.Configurations
{
    public static class RateLimiterConfig
    {
        private const string DefaultPolicyName = "GlobalPolicy";

        public static IServiceCollection AddRateLimiterPolicy(this IServiceCollection services, IHostEnvironment env)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.Response.ContentType = "application/json";

                    var response = APIResponseDTO.Fail("You have exceeded the allowed request limit. Please try again later.");

                    await context.HttpContext.Response.WriteAsJsonAsync(response, cancellationToken: token);
                };

                var permitLimit = env.IsDevelopment() ? 20 : 5;
                var window = env.IsDevelopment() ? TimeSpan.FromSeconds(1) : TimeSpan.FromSeconds(10);

                options.AddPolicy(DefaultPolicyName, httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "global",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = permitLimit,
                            Window = window,
                            QueueLimit = 0,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        }));
            });

            return services;
        }

        public static string GetPolicyName() => DefaultPolicyName;
    }

}
