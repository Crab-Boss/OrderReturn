using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace OrderReturn.Data
{
    public class NullOrderReturnDbSchemaMigrator : IOrderReturnDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}
