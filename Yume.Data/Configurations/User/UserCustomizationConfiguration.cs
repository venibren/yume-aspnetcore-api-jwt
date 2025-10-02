using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Yume.Enums;

namespace Yume.Data.Configurations.User
{
    internal sealed class UserCustomizationConfiguration : IEntityTypeConfiguration<Entities.User.UserCustomization>
    {
        public void Configure(EntityTypeBuilder<Entities.User.UserCustomization> entity)
        {
            entity.ToTable("UserCustomizations");

            entity.HasKey(u => u.Id);

            entity.Property(u => u.Id)
                .IsRequired()
                .HasColumnType("uuid")
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(u => u.UserId)
                .IsRequired();
            entity.HasIndex(u => u.UserId)
                .IsUnique();

            entity.Property(u => u.Avatar);

            entity.Property(u => u.Banner);

            entity.Property(u => u.Description)
                .HasMaxLength(512);

            entity.Property(u => u.Theme)
                .IsRequired()
                .HasDefaultValue(UserThemeEnum.Light);

            entity.HasOne(u => u.User)
                .WithOne(u => u.Customization)
                .HasForeignKey<Entities.User.UserCustomization>(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
