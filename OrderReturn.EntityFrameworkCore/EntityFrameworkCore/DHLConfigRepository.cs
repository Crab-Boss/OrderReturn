using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace OrderReturn.EntityFrameworkCore
{
    public class DHLConfigRepository : EfCoreRepository<OrderReturnDbContext, DHLConfig, int>, IDHLConfigRepository
    {
        public DHLConfigRepository(IDbContextProvider<OrderReturnDbContext> dbContextProvider)
            :base(dbContextProvider)
        {

        }
    }
}
