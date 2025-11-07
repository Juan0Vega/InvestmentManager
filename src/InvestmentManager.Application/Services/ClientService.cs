using InvestmentManager.Application.Common;
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

        public async Task<Client?> GetByIdAsync(int clientId)
        {
            var client = await _clientRepository.GetClientAsync(clientId);
            if (client == null)
                throw new NotFoundException($"No se encontró el cliente con ID {clientId}");

            return client;
        }

        public async Task CreateAsync(Client client)
        {
            if (client == null)
                throw new BusinessException("Los datos del cliente son inválidos.");

            // Verificar si ya existe un cliente con el mismo ID
            var existingClient = await _clientRepository.GetClientAsync(client.ClientId);
            if (existingClient != null)
                throw new BusinessException($"Ya existe un cliente con el ID {client.ClientId}.");

            client.CurrentBalance = ClientInitialAmount.InitialAmount;

            await _clientRepository.UpdateClientAsync(client);
        }

        public async Task<List<Client>> GetAllClients()
        {
            return await _clientRepository.GetAllClients();
        }
    }
}
