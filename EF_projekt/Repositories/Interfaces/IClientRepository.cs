using EF_projekt.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_projekt.Repositories.Interfaces
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Client?> GetClientByEmailAsync(string email);
    }
}
