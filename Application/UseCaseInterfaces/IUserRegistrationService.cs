

using DataTransferObjects.Request.Common;
using DataTransferObjects.Request.User;
using DataTransferObjects.Response.Common;
using DataTransferObjects.Response.User;
using Domain.Entities;

namespace Application.UseCaseInterfaces
{
    public interface IUserRegistrationService : IGenericService<TrnUserRegistration,SaveUserResgistrationDTO, GetUserResponseDTO>
    {
        Task<APIResponseDTO> AddAsync(SaveUserResgistrationDTO dto);
        Task<APIResponseDTO<DataTableResponseDTO<GetUserResponseDTO>>> GetAllAsync(GetUserRequestListDTO dto);
        Task<APIResponseDTO<GetUserResponseDTO>> GetUserDetails(string email);
        Task<APIResponseDTO> DeleteAsync(Guid Id);
    }
}
