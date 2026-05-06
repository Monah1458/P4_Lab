using EF_projekt.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_projekt.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByClientAsync(int clientId);
        Task<Order?> GetOrderWithItemsAsync(int orderId);
        Task<decimal> GetOrderTotalAsync(int orderId);
    }
}
