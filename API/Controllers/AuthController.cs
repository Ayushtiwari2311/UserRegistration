using Application.UseCaseInterfaces;
using Azure;
using DataTransferObjects.Request.APIUser;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
//using Presentation.Examples;
//using Swashbuckle.AspNetCore.Filters;

namespace Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthService _authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterAPIUserRequestModel model)
        {
            var response = await _authService.RegisterAsync(model);
            return StatusCode(((int)response.StatusCode),response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestModel model)
        {
            var response = await _authService.LoginAsync(model);
            return StatusCode(((int)response.StatusCode), response);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            var response = await _authService.LogoutAsync();
            return StatusCode(((int)response.StatusCode), response);
        }
    }
}
