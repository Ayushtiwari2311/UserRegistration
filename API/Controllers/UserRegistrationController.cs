using Application.UseCaseInterfaces;
using DataTransferObjects.Request.Common;
using DataTransferObjects.Request.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Presentation.Controllers
{
    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class UserRegistrationController(IUserRegistrationService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetUserRequestListDTO dto)
        { 
            var result = await service.GetAllAsync(dto);
            return StatusCode(((int)result.StatusCode),result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await service.GetByIdAsync(id);
            return StatusCode(((int)result.StatusCode), result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] SaveUserResgistrationDTO dto)
        {
            var result = await service.AddAsync(dto);
            return StatusCode(((int)result.StatusCode), result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(Guid id, [FromForm] PatchUserRegistrationDTO dto)
        {
            var result = await service.PatchAsync(id, dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateUserRegistrationDTO dto)
        {
            dto.Id = id;
            var result = await service.UpdateAsync(id,dto);
            return StatusCode(((int)result.StatusCode), result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await service.DeleteAsync(id);
            return StatusCode(((int)result.StatusCode), result);
        }
    }
}
