using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace OrderReturn.EntityFrameworkCore
{
    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */
    public class OrderReturnMigrationsDbContextFactory : IDesignTimeDbContextFactory<OrderReturnMigrationsDbContext>
    {
        public OrderReturnMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<OrderReturnMigrationsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Default"));

            return new OrderReturnMigrationsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                //.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Duozan.Easyfx.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
