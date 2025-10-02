using Microsoft.AspNetCore.Identity;

namespace Yume.Data.Entities.Auth
{
    public class AuthUser
    {
        public Guid Id { get; } = Guid.NewGuid();

        public required Guid UserId { get; set; }

        [ProtectedPersonalData]
        public required string PasswordHash { get; set; }

        public bool MfaEmailEnabled { get; set; } = false;

        public bool MfaTotpEnabled { get; set; } = false;

        public bool MfaBackupEnabled { get; set; } = false;

        public virtual User.User User { get; set; }

        public virtual AuthUserMfaTotp? MfaTotp { get; set; }

        public virtual ICollection<AuthUserMfaBackup>? MfaBackup { get; set; }

        public virtual ICollection<AuthUserHistory>? History { get; set; }
    }
}
