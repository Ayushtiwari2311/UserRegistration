﻿namespace Presentation.Configurations
{
    public class JwtFromCookieMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtFromCookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Cookies.TryGetValue("X-Access-Token", out var token))
            {
                context.Request.Headers["Authorization"] = $"Bearer {token}";
            }

            await _next(context);
        }
    }
}
