using Domain.DTOs;

namespace API.Middlewares
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorLoggingMiddleware> _logger;

        public ErrorLoggingMiddleware(RequestDelegate next, ILogger<ErrorLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                ResponseDTO response = new ResponseDTO() { Message = "oops! something went wrong please try again!" };
                //_logger.LogError(ex, "Unhandled exception occurred");
                context.Response.StatusCode = 200;
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
