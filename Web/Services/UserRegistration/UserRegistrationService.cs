using DataTransferObjects.Request.User;
using DataTransferObjects.Response.Common;
using DataTransferObjects.Response.User;
using MVC.Helper;

namespace MVC.Services.UserRegistration
{
    public class UserRegistrationService : IUserRegistrationService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;
        public UserRegistrationService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _contextAccessor = contextAccessor;
        }

        public async Task<APIResponseDTO<DataTableResponseDTO<GetUserResponseDTO>>> GetAllRegisteredUserAsync(GetUserRequestListDTO dto)
        {
            var queryParams = new List<string>();

            if (dto.StateId.HasValue)
                queryParams.Add($"StateId={dto.StateId.Value}");

            if (dto.CityId.HasValue)
                queryParams.Add($"CityId={dto.CityId.Value}");

            queryParams.Add($"Start={dto.Start}");
            queryParams.Add($"Length={dto.Length}");
            queryParams.Add($"Draw={dto.Draw}");
            queryParams.Add($"SortColumn={dto.SortColumn}");
            queryParams.Add($"SortDirection={dto.SortDirection}");

            if (!string.IsNullOrEmpty(dto.searchValue))
                queryParams.Add($"searchValue={Uri.EscapeDataString(dto.searchValue)}");

            var queryString = string.Join("&", queryParams);

            var response = await _httpClient.GetAsync($"/UserRegistration?{queryString}");
            var apiResponse = await response.ReadApiResponseAsync<DataTableResponseDTO<GetUserResponseDTO>>(_contextAccessor.HttpContext);
            return apiResponse;
        }
    }
}
