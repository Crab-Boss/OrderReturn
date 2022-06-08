using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace OrderReturn
{
    [DependsOn(
    typeof(AbpValidationModule),
        typeof(AbpLocalizationModule)
    )]
    public class OrderReturnDomainSharedModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {

        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<OrderReturnDomainSharedModule>();
            });

            //Configure<AbpLocalizationOptions>(options =>
            //{
            //    options.Resources
            //        .Add<OrderReturnResource>("en")
            //        .AddBaseTypes(typeof(AbpValidationResource))
            //        .AddVirtualJson("/Localization/OrderReturn");

            //    options.DefaultResourceType = typeof(OrderReturnResource);
            //});

            //Configure<AbpExceptionLocalizationOptions>(options =>
            //{
            //    options.MapCodeNamespace("OrderReturn", typeof(OrderReturnResource));
            //});
        }
    }
}