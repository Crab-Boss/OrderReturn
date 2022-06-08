using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;


namespace OrderReturn.EntityFrameworkCore
{
    [DependsOn(
    typeof(OrderReturnDomainModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule)
    )]
    public class OrderReturnEntityFrameworkCoreModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<OrderReturnDbContext>(options =>
            {
                /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
                //options.AddDefaultRepositories(includeAllEntities: true);
                options.AddDefaultRepositories<IOrderReturnDbContext>(true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                /* The main point to change your DBMS.
                 * See also OrderReturnMigrationsDbContextFactory for EF Core tooling. */
                options.UseSqlServer();
            });
        }
    }
}
