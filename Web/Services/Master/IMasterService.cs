using DataTransferObjects.Response.Common;

namespace Web.Services.Master
{
    public interface IMasterService
    {
        Task<APIResponseDTO<List<DropDownResponseDTO>>> GetAllDropDown(string dDName, int parentId);
    }
}
