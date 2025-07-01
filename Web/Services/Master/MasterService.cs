using DataTransferObjects.Response.Common;
using System.Net.Http;
using MVC.Helper;

namespace MVC.Services.Master
{
    public class MasterService : IMasterService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;
        public MasterService(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _contextAccessor = contextAccessor;
        }

        public async Task<APIResponseDTO<List<DropDownResponseDTO>>> GetAllDropDown(string dDName,int parentId) {
            var response = await _httpClient.GetAsync($"/Masters/{dDName}{(parentId > 0 ? $"/{parentId}" : "")}");
            var apiResponse = await response.ReadApiResponseAsync<List<DropDownResponseDTO>>(_contextAccessor.HttpContext);
            return apiResponse;
        }

    }
}
