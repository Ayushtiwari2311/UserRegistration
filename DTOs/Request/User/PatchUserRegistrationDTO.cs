using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Request.User
{
    public class PatchUserRegistrationDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? ContactNo { get; set; }
        public int? State { get; set; }
        public int? City { get; set; }
        public List<int>? Hobbies { get; set; }
        public IFormFile? Photo { get; set; }
        public string? PhotoPath { get; set; }
    }
}
