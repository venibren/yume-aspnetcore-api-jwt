using System.ComponentModel.DataAnnotations;
using Yume.Common.Helpers;

namespace Yume.Attributes
{
    public partial class FullUsernameAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return new ValidationResult("Username cannot be null.");

            string? input = value as string;

            if (!RegexHelper.RgxUsername().IsMatch(input ?? string.Empty))
            {
                return new ValidationResult($"String '{input}' could not resolve to Username format. Example: 'username#1234'");
            }

            return ValidationResult.Success;
        }
    }
}
