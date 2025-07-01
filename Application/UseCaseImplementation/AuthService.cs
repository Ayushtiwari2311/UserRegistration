using Application.UseCaseInterfaces;
using DataTransferObjects.Request.APIUser;
using DataTransferObjects.Response.Common;
using Domain.Entities;
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
                            IConfiguration _configuration) : IAuthService
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

        public async Task<APIResponseDTO<string>> LoginAsync(LoginUserRequestModel model) {
            APIResponseDTO<string> response;

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                response = APIResponseDTO<string>.Fail($"No user found by {model.Email}",HttpStatusCode.Unauthorized);
            }
            else
            {
                if (!await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    response = APIResponseDTO<string>.Fail($"Invalid Password!", HttpStatusCode.Unauthorized);
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

                    response = APIResponseDTO<string>.Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
            }

            return response;
        }
    }
}
