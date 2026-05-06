using EF_projekt.Relations;
using EF_projekt.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EF_projekt.Repositories.ProductRepository;

namespace EF_projekt.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ShopDbContext context) : base(context) { }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _dbSet
                .Where(p => p.Categories.Any(c => c.IdCategory == categoryId))      
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsWithActiveDiscountsAsync()
        {
            return await _dbSet
                .Include(p => p.Discounts)
                .Where(p => p.Discounts.Any(d => d.StartDate <= DateTime.UtcNow && d.EndDate >= DateTime.UtcNow))
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsBySupplierAsync(int supplierId)
        {
            return await _dbSet
                .Where(p => p.IdSupplier == supplierId)
                .Include(p => p.Supplier)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCountryAsync(string country)
        {
            return await _dbSet
                .Where(p => p.Country != null && p.Country.Equals(country, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }

        public async Task<decimal> GetProductPriceAsync(int productId)
        {
            var product = await _dbSet.FindAsync(productId);
            if (product == null)
                throw new KeyNotFoundException($"Product not found");
            return product.Price;
        }
    }
}
