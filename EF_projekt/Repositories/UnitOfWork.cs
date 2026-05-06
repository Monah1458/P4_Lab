using EF_projekt.Relations;
using EF_projekt.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_projekt.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShopDbContext _context;
        private bool _disposed;

        private IProductRepository? _products;
        private IRepository<Category>? _categories;
        private IRepository<Supplier>? _suppliers;
        private IClientRepository? _clients;
        private IBasketRepository? _baskets;
        private IBasketItemRepository? _basketItems;
        private IOrderRepository? _orders;
        private IRepository<OrderItem>? _orderItems;
        private IDiscountRepository? _discounts;

        public UnitOfWork(ShopDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IProductRepository Products =>
            _products ??= new ProductRepository(_context);

        public IRepository<Category> Categories =>
            _categories ??= new Repository<Category>(_context);
        public IRepository<Supplier> Suppliers =>
            _suppliers ??= new Repository<Supplier>(_context);

        public IClientRepository Clients =>
            _clients ??= new ClientRepository(_context);

        public IBasketRepository Baskets =>
            _baskets ??= new BasketRepository(_context);

        public IBasketItemRepository BasketItems =>
            _basketItems ??= new BasketItemRepository(_context);

        public IOrderRepository Orders =>
            _orders ??= new OrderRepository(_context);

        public IRepository<OrderItem> OrderItems =>
            _orderItems ??= new Repository<OrderItem>(_context);

        public IDiscountRepository Discounts =>
            _discounts ??= new DiscountRepository(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
