

using DataTransferObjects.Request.Common;
using DataTransferObjects.Request.User;
using DataTransferObjects.Response.Common;
using DataTransferObjects.Response.User;

namespace Application.UseCaseInterfaces
{
    public interface IUserRegistrationService
    {
        Task<APIResponseDTO<DataTableResponseDTO<GetUserResponseDTO>>> GetAllAsync(GetUserRequestListDTO dto);
        Task<APIResponseDTO> AddAsync(SaveUserResgistrationDTO dto, FileUploadRequestDto photoDto);
        Task<APIResponseDTO<GetUserResponseDTO>> GetUserDetails(string email);
    }
}
