using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Yume.Data.Entities.Auth;

namespace Yume.Data.Configurations.Auth
{
    internal sealed class AuthUserConfiguration : IEntityTypeConfiguration<AuthUser>
    {
        public void Configure(EntityTypeBuilder<AuthUser> entity)
        {
            entity.ToTable("AuthUsers");

            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id)
                .IsRequired()
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(u => u.UserId)
                .IsRequired();
            entity.HasIndex(u => u.UserId)
                .IsUnique();

            entity.Property(u => u.PasswordHash)
                .IsRequired();

            entity.Property(u => u.MfaEmailEnabled)
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(u => u.MfaTotpEnabled)
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(u => u.MfaBackupEnabled)
                .IsRequired()
                .HasDefaultValue(false);

            // One to one Security table connection
            entity.HasOne(u => u.User)
                .WithOne(u => u.Auth)
                .HasForeignKey<AuthUser>(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // One to one TOTP table connection
            entity.HasOne(u => u.MfaTotp)
                .WithOne(u => u.User)
                .HasForeignKey<AuthUserMfaTotp>(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_auth_user_mfatotp_user_id");

            // One to many Backup Codes table connection
            entity.HasMany(u => u.MfaBackup)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_auth_user_mfabackup_user_id");

            // One to many Login History table connection
            entity.HasMany(u => u.History)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_auth_user_history_user_id");
        }
    }
}
