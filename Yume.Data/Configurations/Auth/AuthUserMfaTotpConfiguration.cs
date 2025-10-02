using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Yume.Data.Entities.Auth;

namespace Yume.Data.Configurations.Auth
{
    internal sealed class AuthUserMfaTotpConfiguration : IEntityTypeConfiguration<AuthUserMfaTotp>
    {
        public void Configure(EntityTypeBuilder<AuthUserMfaTotp> entity)
        {
            entity.ToTable("AuthUserMfaTotps");

            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id)
                .IsRequired()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(u => u.UserId)
                .IsRequired();
            entity.HasIndex(u => u.UserId);

            entity.Property(u => u.SecretHash)
                .IsRequired();

            entity.Property(u => u.CreatedDate)
                .IsRequired()
                .HasColumnType("timestamptz")
                .HasDefaultValue(DateTimeOffset.UtcNow);
        }
    }
}
