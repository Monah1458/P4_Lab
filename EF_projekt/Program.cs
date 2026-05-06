using EF_projekt;
using EF_projekt.Relations;
using EF_projekt.Repositories;
using EF_projekt.Repositories.Interfaces;
using EF_projekt.Services;
using EF_projekt.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EF_projekt
{
    
    class Program
    {
        static async Task Main(string[] args)
        {

            // 1. Build host with configuration and logging
            using IHost host = CreateHostBuilder(args).Build();

            // 2. Ensure database is created (migrations applied)
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ShopDbContext>();
                await context.Database.EnsureCreatedAsync();
                // Optionally: await context.Database.MigrateAsync(); if using migrations
            }

            // 3. Run the demo logic
            await RunDemoAsync(host.Services);
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((context, services) =>
        {
            var connectionString = "Server=DESKTOP-GP9HSQ1\\SQLEXPRESS;Database=ShopDb;Trusted_Connection=True;TrustServerCertificate=True;";
            services.AddDbContext<ShopDbContext>(options =>
                options.UseSqlServer(connectionString));

                services.AddScoped<IUnitOfWork, UnitOfWork>();


                   services.AddScoped<IDiscountService, DiscountService>();
                   services.AddScoped<IProductService, ProductService>();
                   services.AddScoped<IBasketService, BasketService>();
                   services.AddScoped<IOrderService, OrderService>();
                   services.AddScoped<IClientService, ClientService>();
                   services.AddScoped<ICategoryService, CategoryService>();
                   services.AddScoped<ISupplierService, SupplierService>();              
               });

        static async Task RunDemoAsync(IServiceProvider services)
        {
            
            using var scope = services.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var clientService = scope.ServiceProvider.GetRequiredService<IClientService>();
            var basketService = scope.ServiceProvider.GetRequiredService<IBasketService>();
            var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
            var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
            var discountService = scope.ServiceProvider.GetRequiredService<IDiscountService>();
            var categoryService = scope.ServiceProvider.GetRequiredService<ICategoryService>();
            var supplierService = scope.ServiceProvider.GetRequiredService<ISupplierService>();


            try
            {
                var canConnect = await scope.ServiceProvider.GetRequiredService<ShopDbContext>().Database.CanConnectAsync();
                logger.LogInformation($"Database connected: {canConnect}");
                             

                var electronics = await categoryService.CreateCategoryAsync("Electronics");
                var books = await categoryService.CreateCategoryAsync("Books");

                var supplier = await supplierService.CreateSupplierAsync(new Supplier 
                                                                        { SupplierName = "TechSupplier", 
                                                                        Country = "China" });

                var laptop = await productService.CreateProductAsync(new Product
                {
                    Name = "Laptop",
                    Price = 1200.99m,
                    Country = "USA",
                    IdSupplier = supplier.IdSupplier,
                    Categories = new List<Category> { electronics }
                });
                
                var mouse = await productService.CreateProductAsync(new Product
                {
                    Name = "Wireless Mouse",
                    Price = 25.50m,
                    Country = "China",
                    IdSupplier = supplier.IdSupplier,
                    Categories = new List<Category> { electronics }
                });

                var book = await productService.CreateProductAsync(new Product
                {
                    Name = "C# Programming",
                    Price = 49.99m,
                    Country = "UK",
                    IdSupplier = supplier.IdSupplier,
                    Categories = new List<Category> { books }
                });

                var discount = await discountService.CreateDiscountAsync(new Discount
                {

                    Percentage = 10,
                    StartDate = DateTime.UtcNow.AddDays(-1),
                    EndDate = DateTime.UtcNow.AddDays(30)
                });

                await discountService.AddDiscountToProductAsync(laptop.IdProduct, discount.IdDiscount);

                var client = await clientService.CreateClientAsync(new Client { Email = "john@example.com" });

                var basket = await basketService.GetBasketByClientIdAsync(client.IdClient);

                await basketService.AddItemToBasketAsync(basket.IdBasket, laptop.IdProduct, 1);
                await basketService.AddItemToBasketAsync(basket.IdBasket, laptop.IdProduct, 1);
                await basketService.AddItemToBasketAsync(basket.IdBasket, mouse.IdProduct, 2);
                await basketService.AddItemToBasketAsync(basket.IdBasket, book.IdProduct, 1);

                var basketTotal = await basketService.GetBasketTotalAsync(basket.IdBasket);
                logger.LogInformation("Basket total after discounts: {Total:C}", basketTotal);

                var order = await basketService.BasketToOrderAsync(basket.IdBasket);
                

                var fullOrder = await orderService.GetOrderByIdAsync(order.IdOrder);
                Console.WriteLine("\n--- Order Summary ---");
                foreach (var item in fullOrder.OrderItems)
                {
                    Console.WriteLine($"  {item.Quantity} x {item.Product.Name} = {item.Price:C} each");
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during the demo");
            }
        }
    }


}