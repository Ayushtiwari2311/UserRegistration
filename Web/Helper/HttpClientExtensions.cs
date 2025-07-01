using System.Net;
using System.Text.Json;
using DataTransferObjects.Response.Common;
using MVC.Models;

namespace MVC.Helper
{
    public static class HttpClientExtensions
    {
        public static async Task<APIResponseDTO<T>> ReadApiResponseAsync<T>(this HttpResponseMessage response, HttpContext? httpContext = null)
        {
            var contentStream = await response.Content.ReadAsStreamAsync();

            try
            {
                var result = await JsonSerializer.DeserializeAsync<APIResponseDTO<T>>(contentStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase // optional, for clarity
                });

                if (response.StatusCode == HttpStatusCode.Unauthorized && httpContext != null)
                {
                    // Clear token cookie
                    httpContext.Response.Cookies.Delete("jwt_token");

                    // Do a redirect (only works if still in pipeline)
                    httpContext.Response.Redirect("/Auth/Login");
                }


                // Handle null (edge case: empty body or deserialization failed silently)
                return result ?? APIResponseDTO<T>.Fail("Empty or invalid response", response.StatusCode);
            }
            catch (JsonException ex)
            {
                return APIResponseDTO<T>.Fail($"Deserialization error: {ex.Message}", response.StatusCode);
            }
        }
    }
}
