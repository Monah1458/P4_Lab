using EF_projekt.Relations;
using EF_projekt.Repositories.Interfaces;
using EF_projekt.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_projekt.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;      
        }
        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _unitOfWork.Orders.GetByIdAsync(orderId);
        }

        
        public async Task<IEnumerable<Order>> GetOrdersByClientAsync(int clientId)
        {
            return await _unitOfWork.Orders.GetOrdersByClientAsync(clientId);
        }
        public async Task DeleteOrderByIdAsync(int orderId)
        {
            await _unitOfWork.Orders.DeleteByIdAsync(orderId);
        }
    }
}
