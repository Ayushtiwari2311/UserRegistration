using DataTransferObjects.Response.Common;

namespace Application.UseCaseInterfaces
{
    public interface IMastersService
    {
        Task<APIResponseDTO<IEnumerable<DropDownResponseDTO>>> GetStatesAsync();
        Task<APIResponseDTO<IEnumerable<DropDownResponseDTO>>> GetHobbiesAsync();
        Task<APIResponseDTO<IEnumerable<DropDownResponseDTO>>> GetGendersAsync();
        Task<APIResponseDTO<IEnumerable<DropDownResponseDTO>>> GetCitiesAsync(int stateId);
    }
}
