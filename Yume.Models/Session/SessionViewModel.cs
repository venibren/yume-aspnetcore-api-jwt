using Yume.Enums;

namespace Yume.Models.Session
{
    public class SessionViewModel
    {
        public required Guid Id { get; set; }

        public required Guid OwnerId { get; set; }

        public required string Token { get; set; }

        public required DateTime CreatedAt { get; set; }

        public required DateTime ExpireAt { get; set; }

        public required SessionTypeEnum Type { get; set; }

        public required string IpAddress { get; set; }

        public required string Agent { get; set; }

        public static SessionViewModel Parse(SessionModel session)
        {
            return new SessionViewModel
            {
                Id = session.Id,
                OwnerId = session.OwnerId,
                Token = session.Token,
                CreatedAt = session.CreatedAt,
                ExpireAt = session.ExpireAt,
                Type = session.Type,
                IpAddress = session.IpAddress,
                Agent = session.Agent
            };
        }
    }
}
