using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CustomAttributes
{
    internal class ValidDateOfBirthAttribute : ValidationAttribute
    {
        public ValidDateOfBirthAttribute()
        {
            ErrorMessage = "User must be at least 18 years old and DOB cannot be in the future.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dob)
            {
                if (dob > DateTime.Today)
                {
                    return new ValidationResult("Date of Birth cannot be in the future.");
                }

                var age = DateTime.Today.Year - dob.Year;
                if (dob > DateTime.Today.AddYears(-age)) age--;

                if (age < 18)
                {
                    return new ValidationResult("User must be at least 18 years old.");
                }

                return ValidationResult.Success!;
            }

            return new ValidationResult("Invalid Date of Birth.");
        }
    }
}
