using DataTransferObjects.Request.APIUser;
using DataTransferObjects.Response.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCaseInterfaces
{
    public interface IAuthService
    {
        Task<APIResponseDTO> RegisterAsync(RegisterAPIUserRequestModel model);
        Task<APIResponseDTO> LoginAsync(LoginUserRequestModel model);

        Task<APIResponseDTO> LogoutAsync();
    }
}
