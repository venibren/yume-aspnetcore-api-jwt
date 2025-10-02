using System.Text.Json.Serialization;

namespace Yume.Models.User
{
    public class UserTokenModel
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        public required Guid Session { get; set; }
        public required string Token { get; set; }
        public required DateTime ExpiryDate { get; set; }
    }
}
