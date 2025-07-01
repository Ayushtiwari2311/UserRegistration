using Presentation.CustomAttributes;
using DataTransferObjects.Request.User;

namespace Presentation.DTOs
{
    /// <summary>
    /// Model for add user.
    /// </summary>
    public class AddUserRegistrationDTO : SaveUserResgistrationDTO
    {
        /// <summary>User's Photo.</summary>
        [ValidFile(new[] { ".jpg", ".png" }, maxFileSizeMB: 2)]
        public IFormFile Photo { get; set; }
    }
}
