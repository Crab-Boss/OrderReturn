using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace OrderReturn
{
    [DependsOn(typeof(OrderReturnApplicationContractsModule))]
    public class OrderReturnHttpApiModule :AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(OrderReturnHttpApiModule).Assembly);
            });
        }
    }
}
