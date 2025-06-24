using DataTransferObjects.Request.User;
using DataTransferObjects.Response.Common;
using Domain.Entities;

namespace Domain.RepositoryInterfaces
{
    public interface IUserRegistrationRepository
    {
        Task<DataTableResponseDTO<TrnUserRegistration>> GetAllAsync(GetUserRequestListDTO dto);
        Task AddAsync(TrnUserRegistration userRegistration);
        Task<bool> CheckUserExistsByEmail(string email);
        Task<APIResponseDTO<TrnUserRegistration>> GetUserDetails(string email);
    }
}
