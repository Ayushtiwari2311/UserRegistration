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
            if (!response.IsSuccess)
                return StatusCode(((int)response.StatusCode),response);
            return Ok(response);
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <param name="model">Login credentials</param>
        /// <returns>JWT token if valid</returns>
        [HttpPost("login")]
        //[SwaggerRequestExample(typeof(LoginRequest), typeof(LoginRequestExample))]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestModel model)
        {
            var response = await _authService.LoginAsync(model);
            if (!response.IsSuccess)
                return StatusCode(((int)response.StatusCode), response);
            return Ok(response);
        }
    }
}
