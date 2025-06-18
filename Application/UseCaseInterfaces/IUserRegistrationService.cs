using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTOs;
using Domain.Entities;

namespace Application.UseCaseInterfaces
{
    public interface IUserRegistrationService
    {
        Task<DTODataTablesResponse<GetUserRegistrationDTO>> GetAllAsync(GetUserRegistrationListDTO dto);
        Task<ResponseDTO> AddAsync(SaveUserResgistrationDTO dto, FileUploadDto photoDto);
        Task<ResponseDTO<GetUserRegistrationDTO>> GetUserDetails(string email);
    }
}
