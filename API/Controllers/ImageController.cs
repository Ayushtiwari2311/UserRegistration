using Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetImage(string folderName, string fileName)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", folderName);
            string filePath = Path.Combine(folderPath, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                string dummyPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
                string dummyFilePath = Path.Combine(dummyPath, "dummyUser.png");
                var dummyFileBytes = System.IO.File.ReadAllBytes(dummyFilePath);
                return File(dummyFileBytes, "image/jpeg");
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "image/jpeg");
        }
    }
}
