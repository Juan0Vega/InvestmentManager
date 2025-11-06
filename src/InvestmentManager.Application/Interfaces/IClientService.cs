using InvestmentManager.Domain.Entities;

namespace InvestmentManager.Application.Interfaces
{
    public interface IClientService
    {
        Task<Client?> GetByIdAsync(string clientId);
        Task CreateAsync(Client client);
    }
}
