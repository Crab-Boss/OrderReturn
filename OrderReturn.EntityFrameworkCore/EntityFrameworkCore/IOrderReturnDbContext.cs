using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace OrderReturn.EntityFrameworkCore
{
    [ConnectionStringName(OrderReturnDbProperties.ConnectionStringName)]
    public interface IOrderReturnDbContext : IEfCoreDbContext
    {
        public DbSet<OrderReturnHistory> OrderReturns { get; set; }
    }
}
