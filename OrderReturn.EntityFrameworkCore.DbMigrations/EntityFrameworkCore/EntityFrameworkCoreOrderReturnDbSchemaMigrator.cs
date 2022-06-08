using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderReturn.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace OrderReturn.EntityFrameworkCore
{
    public class EntityFrameworkCoreOrderReturnDbSchemaMigrator : IOrderReturnDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;
        public EntityFrameworkCoreOrderReturnDbSchemaMigrator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task MigrateAsync()
        {
            /* We intentionally resolving the EasyfxMigrationsDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            var datase = _serviceProvider
                .GetRequiredService<OrderReturnMigrationsDbContext>()
                .Database;

            var migrations = await datase.GetPendingMigrationsAsync();
            if (migrations.Any())
            {
                await datase.MigrateAsync();
            }
        }
    }
}
