using EF_projekt.Relations;
using EF_projekt.Repositories.Interfaces;

namespace EF_projekt.Services.Interfaces
{
    public interface IOrderService
    {
        public Task<Order> GetOrderByIdAsync(int orderId);
        public Task<IEnumerable<Order>> GetOrdersByClientAsync(int clientId);
        public Task DeleteOrderByIdAsync(int orderId);
    }

}