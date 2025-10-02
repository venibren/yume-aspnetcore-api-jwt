using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Yume.Enums;

namespace Yume.Models.Session
{
    public class SessionModel
    {
        [BsonId]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public required Guid Id { get; set; }

        [BsonElement("ownerId")]
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public required Guid OwnerId { get; set; }

        [BsonElement("token")]
        public required string Token { get; set; }

        [BsonElement("createdAt")]
        public required DateTime CreatedAt { get; set; }

        [BsonElement("expireAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public required DateTime ExpireAt { get; set; }

        [BsonElement("type")]
        public required SessionTypeEnum Type { get; set; }

        [BsonElement("ipAddress")]
        public required string IpAddress { get; set; }

        [BsonElement("agent")]
        public required string Agent { get; set; }
    }
}
