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
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Client> GetClientByIdAsync(int id)
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(id);
            if (client == null) throw new KeyNotFoundException($"Client {id} not found.");
            return client;
        }

        public async Task<Client> GetClientByEmailAsync(string email)
        {
            var client = await _unitOfWork.Clients.Query()
                .FirstOrDefaultAsync(c => c.Email == email);
            if (client == null) throw new KeyNotFoundException($"Client with email {email} not found");
            return client;
        }

        public async Task<Client> CreateClientAsync(Client client)
        {
            if (string.IsNullOrWhiteSpace(client.Email))
                throw new ArgumentException("Email required");
            await _unitOfWork.Clients.AddAsync(client);
            await _unitOfWork.CompleteAsync();     
            return client;
        }

        public async Task UpdateClientAsync(Client client)
        {
            var existing = await GetClientByIdAsync(client.IdClient);
            existing.Email = client.Email;
            _unitOfWork.Clients.Update(existing);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteClientAsync(int id)
        {
            var client = await GetClientByIdAsync(id);
            if (client.Orders!= null || client.Orders.Count>0) {
                throw new ArgumentException("Cant delete client with orders");
            }
            var basket = await _unitOfWork.Baskets.GetBasketByClientIdAsync(id);
            await _unitOfWork.Baskets.ClearBasketAsync(basket.IdBasket);
            await _unitOfWork.Baskets.DeleteByIdAsync(basket.IdBasket);
            await _unitOfWork.Clients.DeleteByIdAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<Basket> GetClientBasketAsync(int clientId)
        {
            
            return await _unitOfWork.Baskets.GetBasketByClientIdAsync(clientId);
        }
    }
}
