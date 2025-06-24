using DataTransferObjects.Response.Common;

namespace Application.UseCaseInterfaces
{
    public interface IMastersService
    {
        Task<IEnumerable<DropDownResponseDTO>> GetGendersAsync();
        Task<IEnumerable<DropDownResponseDTO>> GetHobbiesAsync();
        Task<IEnumerable<DropDownResponseDTO>> GetStatesAsync();
        Task<IEnumerable<DropDownResponseDTO>> GetCitiesAsync(int stateId);
    }
}
