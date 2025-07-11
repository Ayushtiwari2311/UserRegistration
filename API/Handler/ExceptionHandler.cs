﻿using DataTransferObjects.Response.Common;
using Microsoft.AspNetCore.Diagnostics;
namespace Presentation.Handler
{
    internal sealed class ExceptionHandler(ILogger<ExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "Unhandled exception occurred in {Method} {Path}",httpContext.Request.Method,httpContext.Request.Path);
            httpContext.Response.StatusCode = StatusCodes.Status200OK;
            await httpContext.Response.WriteAsJsonAsync(APIResponseDTO.Fail("oops! something went wrong please try again!"),cancellationToken);
            return true;
        }
    }
}
