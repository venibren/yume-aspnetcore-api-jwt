using System.Text.Json.Serialization;

namespace Yume.Models.Client
{
    public class ClientTokenModel
    {
        [JsonIgnore]
        public Guid OwnerId { get; set; }
        public required Guid Session { get; set; }
        public required string Token { get; set; }
        public required DateTime ExpiryDate { get; set; }
    }
}
