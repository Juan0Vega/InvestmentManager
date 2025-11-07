using InvestmentManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentManager.Application.Interfaces
{
    public interface IClientRepository
    {
        Task<Client?> GetClientAsync(int clientId);
        Task UpdateClientAsync(Client client);

        Task<List<Client>> GetAllClients();
        

        //Task AddClient(Client client);
    }

  

}
