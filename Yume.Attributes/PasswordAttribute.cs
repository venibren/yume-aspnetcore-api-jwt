using System.ComponentModel.DataAnnotations;
using Yume.Common.Helpers;

namespace Yume.Attributes
{
    public partial class PasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return new ValidationResult("Password cannot be null.");

            string? input = value as string;

            if (!RegexHelper.RgxPassword().IsMatch(input ?? string.Empty))
            {
                return new ValidationResult($"String '{input}' could not resolve to Password format.");
            }

            return ValidationResult.Success;
        }
    }
}
