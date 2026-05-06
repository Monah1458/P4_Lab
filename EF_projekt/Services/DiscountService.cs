using EF_projekt.Relations;
using EF_projekt.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_projekt.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DiscountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<decimal> GetDiscountedProductPriceAsync(int productId)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product == null)
            {
                throw new ArgumentException($"Product does not exist.");
            }

            var activeDiscounts = await GetActiveDiscountsForProductAsync(productId);
            if (!activeDiscounts.Any())
                return product.Price;

            var bestDiscount = activeDiscounts.MaxBy(d => d.Percentage);
            var discountedPrice = product.Price * (decimal)(1 - bestDiscount.Percentage / 100);
            var finalPrice = Math.Round(discountedPrice, 2, MidpointRounding.AwayFromZero);
         

            return finalPrice;
        }

        private async Task<IEnumerable<Discount>> GetActiveDiscountsForProductAsync(int productId)
        {    
            var product = await _unitOfWork.Products.Query()
                .Include(p => p.Discounts)
                .FirstOrDefaultAsync(p => p.IdProduct == productId);

            if (product?.Discounts == null)
                return Enumerable.Empty<Discount>();

            var now = DateTime.UtcNow;
            return product.Discounts.Where(d => d.StartDate <= now && d.EndDate >= now);
        }
        public async Task<Discount> GetDiscountByIdAsync(int id)
        {
            var discount = await _unitOfWork.Discounts.GetByIdAsync(id);
            if (discount == null) throw new KeyNotFoundException($"Discount {id} not found.");
            return discount;
        }

        public async Task<IEnumerable<Discount>> GetAllDiscountsAsync()
        {
            return await _unitOfWork.Discounts.GetAllAsync();
        }

        public async Task<IEnumerable<Discount>> GetActiveDiscountsAsync()
        {
            var now = DateTime.UtcNow;
            return await _unitOfWork.Discounts.Query()
                .Where(d => d.StartDate <= now && d.EndDate >= now)
                .ToListAsync();
        }

        public async Task<Discount> CreateDiscountAsync(Discount discount)
        {
            if (discount.Percentage <= 0 || discount.Percentage > 100)
                throw new ArgumentException("Percent must be between 1 and 100.");
            if (discount.EndDate <= discount.StartDate)
                throw new ArgumentException("EndDate must be after StartDate.");

            await _unitOfWork.Discounts.AddAsync(discount);
            await _unitOfWork.CompleteAsync();
            
            return discount;
        }

        public async Task UpdateDiscountAsync(Discount discount)
        {
            var existing = await GetDiscountByIdAsync(discount.IdDiscount);
            existing.Percentage = discount.Percentage;
            existing.StartDate = discount.StartDate;
            existing.EndDate = discount.EndDate;
            _unitOfWork.Discounts.Update(existing);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteDiscountAsync(int id)
        {
            await _unitOfWork.Discounts.DeleteByIdAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task AddDiscountToProductAsync(int productId, int discountId)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            var discount = await GetDiscountByIdAsync(discountId);

            if (product == null) throw new KeyNotFoundException($"Product {productId} not found.");
            if (product.Discounts == null) product.Discounts = new List<Discount>();
            if (!product.Discounts.Any(d => d.IdDiscount == discountId))
            {
                product.Discounts.Add(discount);
                await _unitOfWork.CompleteAsync();               
            }
        }

        public async Task RemoveDiscountFromProductAsync(int productId, int discountId)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product?.Discounts == null) return;

            var discount = product.Discounts.FirstOrDefault(d => d.IdDiscount == discountId);
            if (discount != null)
            {
                product.Discounts.Remove(discount);
                await _unitOfWork.CompleteAsync();
            }
        }



    }
}
