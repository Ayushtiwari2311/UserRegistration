

using DataTransferObjects.Request.Common;
using DataTransferObjects.Request.User;
using DataTransferObjects.Response.Common;
using DataTransferObjects.Response.User;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;

namespace Application.UseCaseInterfaces
{
    public interface IUserRegistrationService : IGenericService<TrnUserRegistration,SaveUserResgistrationDTO,UpdateUserRegistrationDTO, GetUserResponseDTO>
    {
        Task<APIResponseDTO> AddAsync(SaveUserResgistrationDTO dto);
        Task<APIResponseDTO<DataTableResponseDTO<GetUserResponseDTO>>> GetAllAsync(GetUserRequestListDTO dto);
        Task<APIResponseDTO<GetUserResponseDTO>> GetByIdAsync(Guid id);
        Task<APIResponseDTO> UpdateAsync(Guid id, UpdateUserRegistrationDTO dto);
        Task<APIResponseDTO> PatchAsync(Guid id, PatchUserRegistrationDTO dto);
        Task<APIResponseDTO> DeleteAsync(Guid Id);
    }
}
