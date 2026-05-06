using EF_projekt.Relations;
using EF_projekt.Repositories.Interfaces;
using EF_projekt.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_projekt.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDiscountService _discountService;
        public ProductService(IUnitOfWork unitOfWork, IDiscountService discountServic)
        {
            _unitOfWork = unitOfWork;
            _discountService = discountServic;
        }
        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _unitOfWork.Products.GetByIdAsync(productId);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _unitOfWork.Products.GetAllAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _unitOfWork.Products.GetProductsByCategoryAsync(categoryId);
        }

        public async Task<IEnumerable<Product>> GetProductsBySupplierAsync(int supplierId)
        {
            return await _unitOfWork.Products.GetProductsBySupplierAsync(supplierId);
        }

        public async Task<IEnumerable<Product>> GetProductsByCountryAsync(string country)
        {
            if (string.IsNullOrWhiteSpace(country))
                throw new ArgumentException("Country cannot be null or empty.", nameof(country));

            return await _unitOfWork.Products.GetProductsByCountryAsync(country);
        }

        public async Task<IEnumerable<Product>> GetProductsWithActiveDiscountsAsync()
        {
            return await _unitOfWork.Products.GetProductsWithActiveDiscountsAsync();
        }

        public async Task<decimal> GetProductPriceAsync(int productId)
        {      
            return await _unitOfWork.Products.GetProductPriceAsync(productId);
        }

        public async Task<decimal> GetDiscountedProductPriceAsync(int productId)
        {
            var product = _discountService.GetDiscountedProductPriceAsync(productId);

            return await product;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
   
            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Product name is required.");

            if (product.Price <= 0)
                throw new ArgumentException("Product price must be greater than zero.");

            var supplier = await _unitOfWork.Suppliers.GetByIdAsync(product.IdSupplier);
            if (supplier == null)
                 throw new ArgumentException($"Supplier {product.IdSupplier} not found.");
        
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CompleteAsync();
            return product;
        }

        public async Task UpdateProductAsync(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var existing = await _unitOfWork.Products.GetByIdAsync(product.IdProduct);
            if (existing == null)
                throw new KeyNotFoundException($"Product {product.IdProduct} not found.");

            
            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.Country = product.Country;
            existing.IdSupplier = product.IdSupplier;
        

            _unitOfWork.Products.Update(existing);
            await _unitOfWork.CompleteAsync();

        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product == null)
                throw new KeyNotFoundException($"Product {productId} not found.");

            _unitOfWork.Products.Delete(product);
            await _unitOfWork.CompleteAsync();      
        }
    }

}