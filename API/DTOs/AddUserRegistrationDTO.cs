using API.CustomAttributes;
using Domain.DTOs;

namespace API.DTOs
{
    public class AddUserRegistrationDTO : SaveUserResgistrationDTO
    {
        [ValidFile(new[] { ".jpg", ".png" }, maxFileSizeMB: 2)]
        public IFormFile Photo { get; set; }
    }
}
