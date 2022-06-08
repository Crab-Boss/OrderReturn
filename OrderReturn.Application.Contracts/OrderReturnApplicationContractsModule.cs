using Volo.Abp.Modularity;

namespace OrderReturn
{
    [DependsOn(
    typeof(OrderReturnDomainSharedModule)
    )]
    public class OrderReturnApplicationContractsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            
        }
    }
}
