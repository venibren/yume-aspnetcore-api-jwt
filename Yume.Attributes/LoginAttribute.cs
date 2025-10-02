using System.ComponentModel.DataAnnotations;
using Yume.Common.Helpers;

namespace Yume.Attributes
{
    public partial class LoginAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return new ValidationResult("Login cannot be null.");

            string? input = value as string;

            if (!RegexHelper.RgxFullUsername().IsMatch(input ?? string.Empty) && !new EmailAddressAttribute().IsValid(input ?? string.Empty))
            {
                return new ValidationResult($"String '{input}' could not resolve to Username or Email format. Example: 'username#1234', 'sample.user@mail.com'");
            }

            return ValidationResult.Success;
        }
    }
}
