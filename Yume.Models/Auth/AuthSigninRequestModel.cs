using System.ComponentModel.DataAnnotations;
using Yume.Attributes;

namespace Yume.Models.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthSigninRequestModel
    {
        /// <summary>
        /// An identifiable credential used for creating a new active session.
        /// Supports: Email and Full Username.
        /// </summary>
        [Required]
        [Login]
        public required string Login { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        [Required]
        public required string Password { get; set; }

        /// <summary>
        /// MFA one-time passcode
        /// </summary>
        public string? Otp { get; set; }

        public override string ToString() => Login;
    }
}
