using System.Text.Json;
using Web.Models;

namespace Web.Helper
{
    public static class HttpClientExtensions
    {
        public static async Task<ApiResponseResult<T>> ReadApiResponseAsync<T>(this HttpResponseMessage response,string? jsonDataKey = null)
        {
            var result = new ApiResponseResult<T>
            {
                StatusCode = response.StatusCode,
                Success = response.IsSuccessStatusCode
            };

            var contentStream = await response.Content.ReadAsStreamAsync();

            try
            {
                using var doc = await JsonDocument.ParseAsync(contentStream);

                if (jsonDataKey != null && doc.RootElement.TryGetProperty(jsonDataKey, out var dataElement))
                {
                    result.Data = JsonSerializer.Deserialize<T>(dataElement.GetRawText(), new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else if (response.IsSuccessStatusCode)
                {
                    result.Data = JsonSerializer.Deserialize<T>(doc.RootElement.GetRawText(), new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    result.ErrorMessage = doc.RootElement.ToString();
                }
            }
            catch (JsonException jsonEx)
            {
                result.ErrorMessage = $"Deserialization failed: {jsonEx.Message}";
            }

            return result;
        }
    }
}
