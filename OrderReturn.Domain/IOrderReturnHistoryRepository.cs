using System;
using Volo.Abp.Domain.Repositories;

namespace OrderReturn
{
    public interface IOrderReturnHistoryRepository : IRepository<OrderReturnHistory,Guid>
    {

    }
}
