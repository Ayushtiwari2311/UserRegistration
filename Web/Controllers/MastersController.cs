using Microsoft.AspNetCore.Mvc;
using MVC.Services.Master;

namespace MVC.Controllers
{
    [Route("[controller]")]
    public class MastersController(IMasterService _masterService) : Controller
    {
        [HttpGet("dropdown")]
        public async Task<IActionResult> GetAllDropDown(string ddName,int? parentId = 0)
        {
            return Ok(await _masterService.GetAllDropDown(ddName,parentId.GetValueOrDefault()));
        }
    }
}
