using Application.UseCaseInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MastersController(IMastersService service) : ControllerBase
    {
        [HttpGet("states")]
        public async Task<IActionResult> GetStates()
        {
            var result = await service.GetStatesAsync();
            return Ok(result);
        }

        [HttpGet("cities/{stateId}")]
        public async Task<IActionResult> GetCities([FromRoute]int stateId)
        {
            var result = await service.GetCitiesAsync(stateId);
            return Ok(result);
        }

        [HttpGet("hobbies")]
        public async Task<IActionResult> GetHobbies()
        {
            var result = await service.GetHobbiesAsync();
            return Ok(result);
        }

        [HttpGet("genders")]
        public async Task<IActionResult> GetGenders()
        {
            var result = await service.GetGendersAsync();
            return Ok(result);
        }
    }
}
