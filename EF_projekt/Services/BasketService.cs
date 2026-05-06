using EF_projekt.Relations;
using EF_projekt.Repositories.Interfaces;
using EF_projekt.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_projekt.Services
{
    public class BasketService : IBasketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDiscountService _discountService;

        public BasketService(IUnitOfWork unitOfWork, IDiscountService pricingService)
        {
            _unitOfWork = unitOfWork;
            _discountService = pricingService;
        }

        public async Task<decimal> GetBasketTotalAsync(int idBasket)
        {
            var basket = await _unitOfWork.Baskets.GetBasketWithItemsAsync(idBasket);
            if (basket?.Items == null) return 0;

            decimal total = 0;
            foreach (var item in basket.Items)
            {

                var price = await _discountService.GetDiscountedProductPriceAsync(item.IdProduct);
                total += price * item.Quantity;
            }
            return total;
        }
        public async Task<Order> BasketToOrderAsync(int idBasket)
        {
            var basket = await _unitOfWork.Baskets.GetBasketWithItemsAsync(idBasket);
            if (basket?.Items == null || !basket.Items.Any())
            {
                throw new InvalidOperationException($"Basket is empty or does not exist.");
            }

            var order = new Order
            {
                IdClient = basket.IdClient,
                OrderDate = DateTime.UtcNow,
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in basket.Items)
            {
                var finalPrice = await _discountService.GetDiscountedProductPriceAsync(item.IdProduct);

                var orderItem = new OrderItem
                {
                    IdProduct = item.IdProduct,
                    Quantity = item.Quantity,
                    Price = finalPrice,
                };
                order.OrderItems.Add(orderItem);
            }
            await _unitOfWork.Orders.AddAsync(order);

            await _unitOfWork.Baskets.ClearBasketAsync(idBasket);

            await _unitOfWork.CompleteAsync();

            return order;
        }


        public async Task<Basket> GetBasketByIdAsync(int basketId)
        {
            var basket = await _unitOfWork.Baskets.GetBasketWithItemsAsync(basketId);
            if (basket == null)
                throw new KeyNotFoundException($"Basket {basketId} not found.");
            return basket;
        }

        public async Task<Basket> GetBasketByClientIdAsync(int clientId)
        {
            var basket = await _unitOfWork.Baskets.GetBasketByClientIdAsync(clientId);
            if (basket == null)
                throw new KeyNotFoundException($"Client {clientId} not found.");

            return basket;
        }

        public async Task<Basket> AddItemToBasketAsync(int basketId, int productId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            var basket = await GetBasketByIdAsync(basketId);
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product == null)
                throw new KeyNotFoundException($"Product {productId} not found.");

            var existingItem = basket.Items.FirstOrDefault(i => i.IdProduct == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                _unitOfWork.BasketItems.Update(existingItem);
            }
            else
            {
                var newItem = new BasketItem
                {
                    IdBasket = basketId,
                    IdProduct = productId,
                    Quantity = quantity,
                    Product = product
                };
                await _unitOfWork.BasketItems.AddAsync(newItem);
                basket.Items.Add(newItem);
            }

            await _unitOfWork.CompleteAsync();
            return basket;
        }

        public async Task<Basket> UpdateItemQuantityAsync(int basketId, int productId, int newQuantity)
        {
            var basket = await GetBasketByIdAsync(basketId);
            var item = basket.Items.FirstOrDefault(i => i.IdProduct == productId);
            if (item == null)
                throw new KeyNotFoundException($"Product {productId} not found in basket.");

            
            item.Quantity += newQuantity;
            _unitOfWork.BasketItems.Update(item);
            await _unitOfWork.CompleteAsync();           
            if (item.Quantity == 0)
            {               
                return await RemoveItemFromBasketAsync(basketId, productId);
            }
            
            return basket;
        }

        public async Task<Basket> RemoveItemFromBasketAsync(int basketId, int productId)
        {
            var basket = await GetBasketByIdAsync(basketId);
            var item = basket.Items.FirstOrDefault(i => i.IdProduct == productId);
            if (item != null)
            {
                _unitOfWork.BasketItems.Delete(item);
                basket.Items.Remove(item);
                await _unitOfWork.CompleteAsync();
                
            }
            return basket;
        }

        public async Task ClearBasketAsync(int basketId)
        {          
              await _unitOfWork.Baskets.ClearBasketAsync(basketId);      
        }


    }
}
