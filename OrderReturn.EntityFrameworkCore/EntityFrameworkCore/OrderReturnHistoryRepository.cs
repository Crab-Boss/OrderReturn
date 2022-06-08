using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace OrderReturn.EntityFrameworkCore
{
    public class OrderReturnHistoryRepository : EfCoreRepository<OrderReturnDbContext,OrderReturnHistory,Guid>, IOrderReturnHistoryRepository
    {
        public OrderReturnHistoryRepository(IDbContextProvider<OrderReturnDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
