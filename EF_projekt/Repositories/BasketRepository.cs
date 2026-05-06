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
    public class BasketRepository : Repository<Basket>, IBasketRepository
    {
        public BasketRepository(ShopDbContext context) : base(context) { }

        public async Task<Basket?> GetBasketWithItemsAsync(int idBasket)
        {
            return await _dbSet
                .Include(b => b.Items)
                    .ThenInclude(bi => bi.Product)
                .FirstOrDefaultAsync(b => b.IdBasket == idBasket);
        }

        public async Task<Basket?> GetBasketByClientIdAsync(int idClient)
        {
            return await _dbSet
                .Include(b => b.Items)
                    .ThenInclude(bi => bi.Product)
                .FirstOrDefaultAsync(b => b.IdClient == idClient);
        }

        public async Task ClearBasketAsync(int idBasket)
        {
            var basket = await GetBasketWithItemsAsync(idBasket);
            if (basket != null && basket.Items != null)
            {
                _context.BasketItems.RemoveRange(basket.Items);
                await _context.SaveChangesAsync();
            }
        }
    }
}
