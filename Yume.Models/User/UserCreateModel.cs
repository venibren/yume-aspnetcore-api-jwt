using System.ComponentModel.DataAnnotations;
using Yume.Attributes;

namespace Yume.Models.User
{
    public class UserCreateModel
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        [Username]
        public required string Username { get; set; }
        [Required]
        [Password]
        public required string Password { get; set; }

        public override string ToString() => Email;
    }
}
