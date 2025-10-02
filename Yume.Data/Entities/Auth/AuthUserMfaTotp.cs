using Microsoft.AspNetCore.Identity;

namespace Yume.Data.Entities.Auth
{
    public class AuthUserMfaTotp
    {
        public Guid Id { get; } = Guid.NewGuid();

        public required Guid UserId { get; set; }

        [ProtectedPersonalData]
        public required string SecretHash { get; set; }

        public DateTimeOffset CreatedDate { get; private set; } = DateTimeOffset.UtcNow;

        public virtual AuthUser User { get; set; }
    }
}
