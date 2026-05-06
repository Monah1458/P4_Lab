using EF_projekt.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_projekt.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IRepository<Category> Categories { get; }
        IRepository<Supplier> Suppliers { get; }
        IClientRepository Clients { get; }
        IBasketRepository Baskets { get; }
        IBasketItemRepository BasketItems { get; }
        IOrderRepository Orders { get; }
        IRepository<OrderItem> OrderItems { get; }
        IDiscountRepository Discounts { get; }

        Task<int> CompleteAsync();
    }
}
