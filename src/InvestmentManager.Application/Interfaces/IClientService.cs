using InvestmentManager.Domain.Entities;

namespace InvestmentManager.Application.Interfaces
{
    public interface IClientService
    {
        Task<Client?> GetByIdAsync(int clientId);
        Task CreateAsync(Client client);

        Task<List<Client>> GetAllClients();
    }
}
