using Yume.Enums;

namespace Yume.Models.User
{
    public class UserModel
    {
        public required Guid Id { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public string? Nickname { get; set; }
        public required string Discriminator { get; set; }
        public string? DisplayName
        {
            get => ToString();
        }

        public required DateTimeOffset CreatedDate { get; set; }
        public required DateTimeOffset UpdatedDate { get; set; }
        public required bool IsActive { get; set; }

        public override string ToString() => $"{Username}#{Discriminator}";
    }
}
