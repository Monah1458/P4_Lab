using EF_projekt.Relations;

namespace EF_projekt.Services.Interfaces
{
    public interface IBasketService
    {
              

        public Task<decimal> GetBasketTotalAsync(int idBasket);


        public Task<Order> BasketToOrderAsync(int idBasket);
        
        Task<Basket> GetBasketByIdAsync(int basketId);
        Task<Basket> GetBasketByClientIdAsync(int clientId);
        Task<Basket> AddItemToBasketAsync(int basketId, int productId, int quantity);
        Task<Basket> UpdateItemQuantityAsync(int basketId, int productId, int newQuantity);
        Task<Basket> RemoveItemFromBasketAsync(int basketId, int productId);
        Task ClearBasketAsync(int basketId);
        

    }
}