using DataTransferObjects.Request.APIUser;
using DataTransferObjects.Response.Common;

namespace MVC.Services.Auth
{
    public interface IAuthService
    {
        Task<APIResponseDTO<string>> LoginAsync(LoginUserRequestModel model);
        Task<APIResponseDTO> RegisterAsync(RegisterAPIUserRequestModel model);
    }
}
