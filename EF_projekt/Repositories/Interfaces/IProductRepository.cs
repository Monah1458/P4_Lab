using EF_projekt.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_projekt.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetProductsWithActiveDiscountsAsync();       
        Task<IEnumerable<Product>> GetProductsBySupplierAsync(int supplierId);
        Task<IEnumerable<Product>> GetProductsByCountryAsync(string country);
        Task<decimal> GetProductPriceAsync(int productId);
    }
}
