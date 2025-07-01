using System.Net.Http.Headers;
using System.Net;

namespace MVC.Helper
{
    public class AuthValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();
            if (path != null && (path.StartsWith("/auth") || path.StartsWith("/css") || path.StartsWith("/js") || path.StartsWith("/images")))
            {
                await _next(context); // Skip login/register/static
                return;
            }

            var token = context.Request.Cookies["jwt_token"];
            if (string.IsNullOrEmpty(token))
            {
                context.Response.Redirect("/auth/login");
                return;
            }
            await _next(context);
        }
    }
}
