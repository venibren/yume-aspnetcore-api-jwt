using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Yume.Data.Entities.Auth;

namespace Yume.Data.Configurations.Auth
{
    internal sealed class AuthUserHistoryConfiguration : IEntityTypeConfiguration<AuthUserHistory>
    {
        public void Configure(EntityTypeBuilder<AuthUserHistory> entity)
        {
            entity.ToTable("AuthUserHistory");

            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id)
                .IsRequired()
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(u => u.UserId)
                .IsRequired();

            entity.Property(u => u.Timestamp)
                .IsRequired()
                .HasColumnType("timestamptz")
                .HasDefaultValue(DateTimeOffset.UtcNow);

            entity.Property(u => u.Success)
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(u => u.Reason)
                .HasMaxLength(256);

            entity.Property(u => u.IpAddress)
                .IsRequired();

            entity.Property(u => u.Agent)
                .IsRequired();
        }
    }
}
