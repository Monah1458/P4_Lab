using EF_projekt.Relations;
using EF_projekt.Repositories.Interfaces;

namespace EF_projekt.Services.Interfaces
{
    public interface IProductService
    {

        public Task<Product> GetProductByIdAsync(int productId);


        public  Task<IEnumerable<Product>> GetAllProductsAsync();

        public  Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        public  Task<IEnumerable<Product>> GetProductsBySupplierAsync(int supplierId);

        public  Task<IEnumerable<Product>> GetProductsByCountryAsync(string country);

        public  Task<IEnumerable<Product>> GetProductsWithActiveDiscountsAsync();

        public Task<decimal> GetProductPriceAsync(int productId);

        public  Task<decimal> GetDiscountedProductPriceAsync(int productId);

        public Task<Product> CreateProductAsync(Product product);

        public Task UpdateProductAsync(Product product);
        

        public Task DeleteProductAsync(int productId);
    
    }
}