using DataTransferObjects.Request.APIUser;
using DataTransferObjects.Response.Common;
using MVC.Helper;
using System.Net.Http;
using System.Xml.Linq;

namespace MVC.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _contextAccessor = contextAccessor;
        }

        public async Task<APIResponseDTO<string>> LoginAsync(LoginUserRequestModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("/auth/login" , model);
            var result = await response.ReadApiResponseAsync<string>(_contextAccessor.HttpContext);

            if (result?.IsSuccess == true && !string.IsNullOrEmpty(result.Data))
            {
                // Store token in cookie (HttpOnly + Secure)
                _contextAccessor.HttpContext?.Response.Cookies.Append("jwt_token", result.Data, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });
            }
            return result;
        }

        public async Task<APIResponseDTO> RegisterAsync(RegisterAPIUserRequestModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("/auth/register", model);
            var result = await response.ReadApiResponseAsync<string>(_contextAccessor.HttpContext);
            return result;

        }
    }
}
