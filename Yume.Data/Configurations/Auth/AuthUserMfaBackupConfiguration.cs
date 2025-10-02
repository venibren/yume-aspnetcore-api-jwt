using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Yume.Data.Entities.Auth;

namespace Yume.Data.Configurations.Auth
{
    internal sealed class AuthUserMfaBackupConfiguration : IEntityTypeConfiguration<AuthUserMfaBackup>
    {
        public void Configure(EntityTypeBuilder<AuthUserMfaBackup> entity)
        {
            entity.ToTable("AuthUserMfaBackups");

            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id)
                .IsRequired()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(u => u.UserId)
                .IsRequired();
            entity.HasIndex(u => u.UserId)
                .IsUnique();

            entity.Property(u => u.CodeHash)
                .IsRequired();

            entity.Property(u => u.CreatedDate)
                .IsRequired()
                .HasColumnType("timestamptz")
                .HasDefaultValue(DateTimeOffset.UtcNow);

            entity.Property(u => u.ActivatedDate)
                .HasColumnType("timestamptz");
        }
    }
}
