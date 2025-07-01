using DataTransferObjects.Request.User;
using DataTransferObjects.Response.Common;
using MVC.Helper;

namespace MVC.Services.UserRegistration
{
    public class UserRegistrationService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;
        public UserRegistrationService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _contextAccessor = contextAccessor;
        }

        //public async Task<APIResponseDTO<List<DropDownResponseDTO>>> GetAllRegisteredUser(GetUserRequestListDTO dto)
        //{
        //    var response = await _httpClient.GetAsync($"/Masters/{dDName}{(parentId > 0 ? $"/{parentId}" : "")}");
        //    var apiResponse = await response.ReadApiResponseAsync<List<DropDownResponseDTO>>(_contextAccessor.HttpContext);
        //    return apiResponse;
        //}
    }
}
