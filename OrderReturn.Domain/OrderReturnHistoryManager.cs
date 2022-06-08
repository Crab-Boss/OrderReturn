using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace OrderReturn
{
    public class OrderReturnHistoryManager : DomainService, IOrderReturnHistoryManager
    {
        protected IOrderReturnHistoryRepository OrderReturnHostoryRepository { get; }
        public OrderReturnHistoryManager(IOrderReturnHistoryRepository orderReturnHostoryRepository)
        {
            OrderReturnHostoryRepository = orderReturnHostoryRepository;
        }

    }
}
