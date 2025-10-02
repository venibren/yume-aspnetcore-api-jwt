using Microsoft.EntityFrameworkCore;
using Yume.Data.Entities.Auth;
using Yume.Data.Entities.User;
using Yume.Data.Factories;

namespace Yume.Data.Contexts
{
    public class PostgreDbContext(DbContextOptions<PostgreDbContext> options) : DbContext(options)
    {
        // Auth
        public DbSet<AuthUser> AuthUsers { get; set; }
        public DbSet<AuthUserMfaTotp> AuthUserMfaTotps { get; set; }
        public DbSet<AuthUserMfaBackup> AuthUserMfaBackups { get; set; }
        public DbSet<AuthUserHistory> AuthUserHistory { get; set; }

        // Role Tables

        // User Tables
        public DbSet<User> Users { get; set; }
        public DbSet<UserCustomization> UserCustomizations { get; set; }

        // Client Tables

        public void InitializeDatabase()
        {
            //// Use the factory to generate test data
            //var userFactory = new UserFactory();
            //var users = userFactory.GenerateUsers(10); // Adjust the count as needed

            //// Add generated data to the database
            //Users.AddRange(users);

            // Save changes to persist the data
            SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("pgcrypto");
            modelBuilder.HasPostgresExtension("citext");

            // Configurations for other entities
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PostgreDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();

            base.OnConfiguring(optionsBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            var now = DateTimeOffset.UtcNow;

            foreach (var entry in ChangeTracker.Entries<User>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(u => u.CreatedDate).CurrentValue = now;
                    entry.Property(u => u.UpdatedDate).CurrentValue = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(u => u.UpdatedDate).CurrentValue = now;
                }
            }

            return await base.SaveChangesAsync(ct);
        }
    }
}