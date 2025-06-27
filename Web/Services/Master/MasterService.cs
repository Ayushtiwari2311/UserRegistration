using DataTransferObjects.Response.Common;
using System.Net.Http;
using Web.Helper;

namespace Web.Services.Master
{
    public class MasterService : IMasterService
    {
        private readonly HttpClient _httpClient;
        public MasterService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<APIResponseDTO<List<DropDownResponseDTO>>> GetAllDropDown(string dDName,int parentId) {
            var response = await _httpClient.GetAsync($"/Masters/{dDName}{(parentId > 0 ? $"/{parentId}" : "")}");
            var apiResponse = await response.ReadApiResponseAsync<List<DropDownResponseDTO>>();
            if (apiResponse.Success)
            {
                return APIResponseDTO<List<DropDownResponseDTO>>.Ok(apiResponse.Data);
            }
            else
            {
                return APIResponseDTO<List<DropDownResponseDTO>>.Fail(apiResponse.ErrorMessage);
            }
        }

    }
}
