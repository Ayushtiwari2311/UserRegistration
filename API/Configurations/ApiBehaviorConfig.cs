using DataTransferObjects.Response.Common;
using Microsoft.AspNetCore.Mvc;

namespace API.Configurations
{
    public static class ApiBehaviorConfig
    {
        public static IServiceCollection AddCustomModelValidationResponse(this IServiceCollection services, IHostEnvironment env)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .SelectMany(kvp =>
                            kvp.Value!.Errors.Select(e =>
                                new { Field = kvp.Key, Message = e.ErrorMessage }));

                    if (env.IsDevelopment())
                    {
                        // HTML response in Development
                        var htmlList = "<ul style='text-align: left; margin-left: 1.5em;'>" +
                                       string.Join("", errors.Select(err =>
                                           $"<li><strong>{err.Field}</strong>: {err.Message}</li>")) +
                                       "</ul>";

                        return new OkObjectResult(APIResponseDTO.Fail(htmlList));
                    }
                    else
                    {
                        // JSON response in Production
                        var jsonErrors = errors
                            .GroupBy(e => e.Field)
                            .ToDictionary(g => g.Key, g => g.Select(e => e.Message).ToArray());

                        return new BadRequestObjectResult(APIResponseDTO.Fail("Model error!"));
                    }
                };
            });

            return services;
        }
    }
}
