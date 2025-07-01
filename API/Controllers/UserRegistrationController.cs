using Presentation.DTOs;
using Application.UseCaseInterfaces;
using DataTransferObjects.Request.Common;
using DataTransferObjects.Request.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserRegistrationController(IUserRegistrationService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetUserRequestListDTO dto)
        { 
            var result = await service.GetAllAsync(dto);
            return Ok(result);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> Get([FromRoute] string email)
        {
            var result = await service.GetUserDetails(email);
            return Ok(result);
        }

        /// <summary>
        /// Add user
        /// </summary>
        /// <returns>Response</returns>
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] AddUserRegistrationDTO dto)
        {
            FileUploadRequestDto photoDto = null;
            if (dto.Photo != null && dto.Photo.Length > 0)
            {
                using var ms = new MemoryStream();
                await dto.Photo.CopyToAsync(ms);
                photoDto = new FileUploadRequestDto
				{
                    FileName = dto.Photo.FileName,
                    ContentType = dto.Photo.ContentType,
                    FileData = ms.ToArray()
                };
            }
            return Ok(await service.AddAsync(dto, photoDto));
        }
    }
}
