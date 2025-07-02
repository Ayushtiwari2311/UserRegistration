using DataTransferObjects.Request.User;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MVC.Services.UserRegistration;

namespace MVC.Controllers
{
    [Route("[controller]")]
    public class UserController(IUserRegistrationService _userService) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery]GetUserRequestListDTO model) 
        {
            return Ok(await _userService.GetAllRegisteredUserAsync(model));
        }
    }
}
