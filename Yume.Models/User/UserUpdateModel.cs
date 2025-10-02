using System.ComponentModel.DataAnnotations;
using Yume.Enums;

namespace Yume.Models.User
{
    public class UserUpdateModel
    {
        [Required]
        public required Guid Id { get; set; }

        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Nickname { get; set; }
        public string? Discriminator { get; set; }
        public UserThemeEnum? Theme { get; set; }

        public override string ToString() => Id.ToString();
    }
}
