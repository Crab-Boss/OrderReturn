using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace OrderReturn.EntityFrameworkCore
{
    [DependsOn(
        typeof(OrderReturnEntityFrameworkCoreModule)
        )]
    public class OrderReturnEntityFrameworkCoreDbMigrationsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {

        }
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<OrderReturnMigrationsDbContext>();
        }
    }
}
