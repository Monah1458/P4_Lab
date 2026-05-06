using EF_projekt.Relations;
using EF_projekt.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_projekt.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ShopDbContext context) : base(context) { }

        public async Task<IEnumerable<Order>> GetOrdersByClientAsync(int clientId)
        {
            return await _dbSet
                .Where(o => o.IdClient == clientId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderWithItemsAsync(int orderId)
        {
            return await _dbSet
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.IdOrder == orderId);
        }

        public async Task<decimal> GetOrderTotalAsync(int orderId)
        {
            var order = await GetOrderWithItemsAsync(orderId);
            if (order == null) return 0;
            return order.OrderItems.Sum(o => o.Quantity * o.Price);
        }
    }
}
