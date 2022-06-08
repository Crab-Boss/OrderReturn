using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace OrderReturn.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class OrderReturnMigrationsDbContext : AbpDbContext<OrderReturnMigrationsDbContext>
    {
        public OrderReturnMigrationsDbContext(DbContextOptions<OrderReturnMigrationsDbContext> options)
            :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureOrderReturn(options => { options.TablePrefix = ""; });
        }
    }
}
