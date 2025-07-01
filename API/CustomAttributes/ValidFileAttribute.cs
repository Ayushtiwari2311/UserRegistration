using System.ComponentModel.DataAnnotations;

namespace Presentation.CustomAttributes
{
    public class ValidFileAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;
        private readonly long _maxFileSizeInBytes;

        public ValidFileAttribute(string[] allowedExtensions, int maxFileSizeMB = 2)
        {
            _allowedExtensions = allowedExtensions.Select(e => e.ToLower()).ToArray();
            _maxFileSizeInBytes = maxFileSizeMB * 1024 * 1024;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file == null)
            {
                return new ValidationResult("File is required.");
            }

            var extension = Path.GetExtension(file.FileName)?.ToLower();
            if (!_allowedExtensions.Contains(extension))
            {
                return new ValidationResult($"Only the following file types are allowed: {string.Join(", ", _allowedExtensions)}");
            }

            if (file.Length > _maxFileSizeInBytes)
            {
                return new ValidationResult($"File size cannot exceed {_maxFileSizeInBytes / (1024 * 1024)} MB.");
            }

            return ValidationResult.Success!;
        }
    }
}
