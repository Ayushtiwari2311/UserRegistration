using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DataTransferObjects.CustomAttributes;
using Domain.CustomAttributes;
using Microsoft.AspNetCore.Http;

namespace DataTransferObjects.Request.User
{
    public class SaveUserResgistrationDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public int Gender { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        [ValidDateOfBirthAttribute]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(150)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Mobile is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
        public string? Mobile { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Contact number must be 10 digits.")]
        public string? ContactNo { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public int State { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public int City { get; set; }

        [Required(ErrorMessage = "At least one hobby must be selected.")]
        [MinLength(1, ErrorMessage = "At least one hobby must be selected.")]
        public List<int> Hobbies { get; set; }

        [ValidFile(new[] { ".jpg", ".png" }, maxFileSizeMB: 2)]
        public IFormFile Photo { get; set; }
        public string? PhotoPath { get; set; }
    }
}
