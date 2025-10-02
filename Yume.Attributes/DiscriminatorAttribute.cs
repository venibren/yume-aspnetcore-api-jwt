using System.ComponentModel.DataAnnotations;
using Yume.Common.Helpers;

namespace Yume.Attributes
{
    public partial class DiscriminatorAttribute : ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return new ValidationResult("Discriminator cannot be null.");

            string? input = value as string;

            if (!RegexHelper.RgxDiscriminator().IsMatch(input ?? string.Empty))
            {
                return new ValidationResult($"String '{input}' could not resolve to Discriminator format.\nExample: '1234'.");
            }

            return ValidationResult.Success;
        }
    }
}
