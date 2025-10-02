using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Yume.Data.Contexts
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PostgreDbContext>
    {
        public PostgreDbContext CreateDbContext(string[] args)
        {
            string webProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Yume.Api");

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(webProjectPath)
                .AddJsonFile("appsettings.Development.json")
                .Build();

            string connectionString = configuration.GetConnectionString("PostgreSql") ?? throw new InvalidOperationException($"Connection string 'PostgreSql' not found.");

            var builder = new DbContextOptionsBuilder<PostgreDbContext>();

            builder.UseNpgsql(connectionString);

            return new PostgreDbContext(builder.Options);
        }
    }
}
