using System.ComponentModel.DataAnnotations;
using Yume.Common.Helpers;

namespace Yume.Attributes
{
    public partial class UsernameAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value as string))
                return new ValidationResult("Username cannot be null.");

            string? input = value as string;

            if (!RegexHelper.RgxUsername().IsMatch(input ?? string.Empty))
            {
                return new ValidationResult($"String '{input}' could not resolve to Username format.\nRequirements:\n- Lowercase a-z,\n- Optional 0-9,\n- Cannot start with a number,\n- Minimum length of 4,\n- Maximum length of 32.\nExample: 'user123'");
            }

            return ValidationResult.Success;
        }
    }
}
