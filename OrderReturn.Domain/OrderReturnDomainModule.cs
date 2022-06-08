using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace OrderReturn
{
    [DependsOn(
    typeof(OrderReturnDomainSharedModule)
    //typeof(AbpSettingManagementDomainModule)
)]
    public class OrderReturnDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();
            Configure<OrderReturnDHLOptions>(options =>
            {
                options.ApplicationId = config["LogisticsConfig:DHL:ApplicationId"];
                options.ApplicationToken = config["LogisticsConfig:DHL:ApplicationToken"];
                options.UserName = config["LogisticsConfig:DHL:UserName"];
                options.Password = config["LogisticsConfig:DHL:Password"];
                options.Url = config["LogisticsConfig:DHL:ProductionUrl"];
//#if DEBUG
//                //Sandbox 沙箱环境
//                options.ApplicationId = "hooya001";
//                options.ApplicationToken = "21Start!";
//                options.UserName = "2222222222_customer";
//                options.Password = "uBQbZ62!ZiBiVVbhc";
//                options.Url = config["LogisticsConfig:DHL:SandboxUrl"];
//#endif

            });
            Configure<OrderReturnGLSOptions>(options =>
            {
                options.Url = config["LogisticsConfig:GLS:Url"];
                options.Apikey = config["LogisticsConfig:GLS:ApiKey"];
            });
        }
    }
}