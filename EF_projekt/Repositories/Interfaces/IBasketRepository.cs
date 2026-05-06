using EF_projekt.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_projekt.Repositories.Interfaces
{
    public interface IBasketRepository : IRepository<Basket>
    {
        Task<Basket?> GetBasketWithItemsAsync(int idBasket);
        Task<Basket?> GetBasketByClientIdAsync(int idClient);
        Task ClearBasketAsync(int idBasket);
    }
}
