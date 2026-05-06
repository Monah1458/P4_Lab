using EF_projekt.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_projekt.Services
{
    public interface IDiscountService
    {
        Task<decimal> GetDiscountedProductPriceAsync(int productId);
        
        Task<Discount> GetDiscountByIdAsync(int id);
        Task<IEnumerable<Discount>> GetAllDiscountsAsync();
        Task<IEnumerable<Discount>> GetActiveDiscountsAsync();
        Task<Discount> CreateDiscountAsync(Discount discount);
        Task UpdateDiscountAsync(Discount discount);
        Task DeleteDiscountAsync(int id);
        Task AddDiscountToProductAsync(int productId, int discountId);
        Task RemoveDiscountFromProductAsync(int productId, int discountId);

    }
}
