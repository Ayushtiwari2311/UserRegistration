using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.Entities;

namespace Domain.RepositoryInterfaces
{
    public interface IUserRegistrationRepository
    {
        Task<DTODataTablesResponse<TrnUserRegistration>> GetAllAsync(GetUserRegistrationListDTO dto);
        Task AddAsync(TrnUserRegistration userRegistration);
        Task<bool> CheckUserExistsByEmail(string email);

        Task<ResponseDTO<TrnUserRegistration>> GetUserDetails(string email);
    }
}
