using Application.UseCaseInterfaces;
using DataTransferObjects.Request.APIUser;
using DataTransferObjects.Response.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCaseImplementation
{
    internal class AuthService(UserManager<ApplicationUser> _userManager,
                            IConfiguration _configuration,
                            IHttpContextAccessor _httpContextAccessor) : IAuthService
    {
        public async Task<APIResponseDTO> RegisterAsync(RegisterAPIUserRequestModel model) {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return APIResponseDTO.Fail(string.Join("| ", errors));
            } 
            return APIResponseDTO.Ok("User registered");
        }

        public async Task<APIResponseDTO> LoginAsync(LoginUserRequestModel model) {
            APIResponseDTO response;

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                response = APIResponseDTO.Fail($"No user found by {model.Email}",HttpStatusCode.Unauthorized);
            }
            else
            {
                if (!await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    response = APIResponseDTO.Fail($"Invalid Password!", HttpStatusCode.Unauthorized);
                }
                else
                {

                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:Issuer"],
                        audience: _configuration["JWT:Audience"],
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                    _httpContextAccessor.HttpContext.Response.Cookies.Append("X-Access-Token", new JwtSecurityTokenHandler().WriteToken(token), new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddHours(3)
                    });

                    response = APIResponseDTO.Ok("User logged in successfully!");
                }
            }

            return response;
        }

        public async Task<APIResponseDTO> LogoutAsync()
        {
            var deleteOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(-1) // Expired
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append("X-Access-Token", "", deleteOptions);

            return APIResponseDTO.Ok("Logged out successfully!");
        }
    }
}
