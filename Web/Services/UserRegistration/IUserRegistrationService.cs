using DataTransferObjects.Request.User;
using DataTransferObjects.Response.Common;
using DataTransferObjects.Response.User;

namespace MVC.Services.UserRegistration
{
    public interface IUserRegistrationService
    {
        Task<APIResponseDTO<DataTableResponseDTO<GetUserResponseDTO>>> GetAllRegisteredUserAsync(GetUserRequestListDTO dto);
    }
}
