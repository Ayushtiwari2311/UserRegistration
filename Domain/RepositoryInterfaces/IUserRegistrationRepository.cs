using DataTransferObjects.Request.User;
using DataTransferObjects.Response.Common;
using Domain.Entities;

namespace Domain.RepositoryInterfaces
{
    public interface IUserRegistrationRepository : IGenericRepository<TrnUserRegistration>
    {
        Task<bool> CheckUserExistsByEmail(string email);
        Task<DataTableResponseDTO<TrnUserRegistration>> GetAllAsync(GetUserRequestListDTO dto);
        Task<APIResponseDTO<TrnUserRegistration>> GetUserDetails(string email);
    }
}
