using System.Threading.Tasks;

namespace OrderReturn.Data
{
    public interface IOrderReturnDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
