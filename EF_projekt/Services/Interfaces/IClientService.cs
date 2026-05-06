using EF_projekt.Relations;

namespace EF_projekt.Services.Interfaces
{
    public interface IClientService
    {
        Task<Client> GetClientByIdAsync(int id);
        Task<Client> GetClientByEmailAsync(string email);
        Task<Client> CreateClientAsync(Client client);
        Task UpdateClientAsync(Client client);
        Task DeleteClientAsync(int id);
        Task<Basket> GetClientBasketAsync(int clientId);
    }
}