using DataTransferObjects.Request.APIUser;
using Microsoft.AspNetCore.Mvc;
using MVC.Services.Auth;

namespace MVC.Controllers
{
    [Route("[controller]/[Action]")]
    public class AuthController(IAuthService _authService) : Controller
    {
        public IActionResult Login() => View();
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginUserRequestModel model)
        {
            return Ok(await _authService.LoginAsync(model));
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterAPIUserRequestModel model)
        {
            return Ok(await _authService.RegisterAsync(model));
        }
    }
}
