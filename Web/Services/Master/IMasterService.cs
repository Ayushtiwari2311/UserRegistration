using DataTransferObjects.Response.Common;

namespace MVC.Services.Master
{
    public interface IMasterService
    {
        Task<APIResponseDTO<List<DropDownResponseDTO>>> GetAllDropDown(string dDName, int parentId);
    }
}
