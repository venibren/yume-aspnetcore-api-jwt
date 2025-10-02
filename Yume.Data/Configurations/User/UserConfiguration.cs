using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Yume.Data.Configurations.User
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<Entities.User.User>
    {
        public void Configure(EntityTypeBuilder<Entities.User.User> entity)
        {
            entity.ToTable("Users");

            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id)
                .IsRequired()
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnType("citext");
            entity.HasIndex(e => e.Email)
                .HasDatabaseName("idx_user_email")
                .IsUnique();

            entity.Property(u => u.EmailVerified)
                .IsRequired()
                .HasDefaultValue(false);

            entity.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(32)
                .HasColumnType("citext");
            entity.HasIndex(e => new { e.Username, e.Discriminator })
                .HasDatabaseName("idx_user_username_full")
                .IsUnique();

            entity.Property(u => u.Nickname)
                .HasMaxLength(32);

            entity.Property(u => u.Discriminator)
                .IsRequired()
                .HasMaxLength(4);

            entity.Property(u => u.CreatedDate)
                .IsRequired()
                .HasColumnType("timestamptz")
                .HasDefaultValue(DateTimeOffset.UtcNow);

            entity.Property(u => u.UpdatedDate)
                .IsRequired()
                .HasColumnType("timestamptz")
                .HasDefaultValue(DateTimeOffset.UtcNow);

            entity.Property(u => u.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // One to one Customization table connection
            entity.HasOne(u => u.Customization)
                .WithOne(u => u.User)
                .HasForeignKey<Entities.User.UserCustomization>(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_user_customization_user_id");
        }
    }
}
