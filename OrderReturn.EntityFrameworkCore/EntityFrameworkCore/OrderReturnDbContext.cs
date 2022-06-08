using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace OrderReturn.EntityFrameworkCore
{
    [ConnectionStringName(OrderReturnDbProperties.ConnectionStringName)]
    public class OrderReturnDbContext : AbpDbContext<OrderReturnDbContext> ,IOrderReturnDbContext
    {
        public DbSet<OrderReturnHistory> OrderReturns { get; set; }
        public DbSet<DHLConfig> DHLConfigs { get; set; }
        public OrderReturnDbContext(DbContextOptions<OrderReturnDbContext> options)
            :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureOrderReturn();
        }
    }
}
