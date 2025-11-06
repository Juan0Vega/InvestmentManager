using InvestmentManager.Application.Interfaces;
using InvestmentManager.Domain.Entities;

namespace InvestmentManager.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<Client?> GetByIdAsync(string clientId)
        {
            return await _clientRepository.GetClientAsync(clientId);
        }

        public async Task CreateAsync(Client client)
        {
            await _clientRepository.UpdateClientAsync(client);
        }
    }
}
