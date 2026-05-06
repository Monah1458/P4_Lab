using EF_projekt.Relations;
using EF_projekt.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_projekt.Repositories
{
    public class DiscountRepository : Repository<Discount>, IDiscountRepository
    {
        public DiscountRepository(ShopDbContext context) : base(context) { }

        public async Task<IEnumerable<Product>> GetProductsWithDiscountsAsync(int idDiscount)
        {
            var discount = await _dbSet
               .Include(d => d.Products)
               .FirstOrDefaultAsync(d => d.IdDiscount == idDiscount);

            return discount?.Products ?? new List<Product>();
        }

        public async Task<TimeSpan?> GetTimeTillEnd(int idDiscount)
        {
            var discount = await _context.Discounts
                .FirstOrDefaultAsync(d => d.IdDiscount == idDiscount);
            if (discount == null || discount.EndDate <= DateTime.UtcNow)
                return null;


            var time= discount.EndDate - DateTime.UtcNow;
            return time > TimeSpan.Zero? time : TimeSpan.Zero;
        }

        public async Task AddDiscountToProductAsync(int idProduct, int idDiscount)
        {
            var product = await _context.Products
                .Include(p => p.Discounts)
                .FirstOrDefaultAsync(p => p.IdProduct == idProduct);

            var discount = await _dbSet.FindAsync(idDiscount);

            if (product != null && discount != null && !product.Discounts.Contains(discount))
            {
                product.Discounts.Add(discount);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveDiscountFromProductAsync(int idProduct, int idDiscount)
        {
            var product = await _context.Products
                .Include(p => p.Discounts)
                .FirstOrDefaultAsync(p => p.IdProduct == idProduct);

            var discount = await _dbSet.FindAsync(idDiscount);

            if (product != null && discount != null && product.Discounts.Contains(discount))
            {
                product.Discounts.Remove(discount);
                await _context.SaveChangesAsync();
            }
        }
    }
}
