using Microsoft.AspNetCore.Identity;

namespace Yume.Data.Entities.Auth
{
    public class AuthUserMfaBackup
    {
        public Guid Id { get; } = Guid.NewGuid();

        public required Guid UserId { get; set; }

        [ProtectedPersonalData]
        public required string CodeHash { get; set; }

        public DateTimeOffset CreatedDate { get; private set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? ActivatedDate { get; set; }

        public virtual AuthUser User { get; set; }

        /// <summary>
        /// Whether the MFA Backup code is available for use.
        /// </summary>
        public bool Enabled => ActivatedDate == null;
    }
}
