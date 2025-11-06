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
        Task<Client?> GetClientAsync(string clientId);
        Task UpdateClientAsync(Client client);

        //Task AddClient(Client client);
    }

  

}
