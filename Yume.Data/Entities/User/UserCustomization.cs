using System.ComponentModel.DataAnnotations;
using Yume.Enums;

namespace Yume.Data.Entities.User
{
    public class UserCustomization
    {
        public Guid Id { get; } = Guid.NewGuid();

        public required Guid UserId { get; set; }

        [MaxLength(2048), Url]
        public string? Avatar { get; set; }

        [MaxLength(2048), Url]
        public string? Banner { get; set; }

        [MaxLength(512)]
        public string? Description { get; set; }

        public UserThemeEnum Theme { get; set; } = UserThemeEnum.Light;

        public virtual User User { get; set; } = default!;
    }
}
