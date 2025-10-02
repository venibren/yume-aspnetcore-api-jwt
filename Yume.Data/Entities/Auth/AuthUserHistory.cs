using Microsoft.AspNetCore.Identity;

namespace Yume.Data.Entities.Auth
{
    public class AuthUserHistory
    {
        public Guid Id { get; } = Guid.NewGuid();

        public required Guid UserId { get; set; }

        public DateTimeOffset Timestamp { get; private set; } = DateTimeOffset.UtcNow;

        public required bool Success { get; set; } = false;

        public string? Reason { get; set; }

        [ProtectedPersonalData]
        public required string IpAddress { get; set; }

        [PersonalData]
        public required string Agent { get; set; }

        public virtual AuthUser User { get; set; }
    }
}
