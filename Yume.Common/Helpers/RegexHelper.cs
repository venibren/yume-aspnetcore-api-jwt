using System.Text.RegularExpressions;

namespace Yume.Common.Helpers
{
    public partial class RegexHelper
    {
        [GeneratedRegex(@"^[a-z][a-z0-9]{2,31}#[0-9]{4}$")]
        public static partial Regex RgxFullUsername();

        [GeneratedRegex(@"^[a-z][a-z0-9]{3,31}$")]
        public static partial Regex RgxUsername();

        [GeneratedRegex(@"^\d{4}$")]
        public static partial Regex RgxDiscriminator();

        [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{8,64}$")]
        public static partial Regex RgxPassword();
    }
}
