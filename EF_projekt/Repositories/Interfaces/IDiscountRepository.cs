using EF_projekt.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_projekt.Repositories.Interfaces
{
    public interface IDiscountRepository : IRepository<Discount>
    {
        Task<IEnumerable<Product>> GetProductsWithDiscountsAsync(int idDiscount);
        Task<TimeSpan?> GetTimeTillEnd(int idDiscount);
        Task AddDiscountToProductAsync(int idProduct, int idDiscount);
        Task RemoveDiscountFromProductAsync(int idProduct, int idDiscount);
    }
}
