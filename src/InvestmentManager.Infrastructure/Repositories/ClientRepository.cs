using InvestmentManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using InvestmentManager.Application.Interfaces;

namespace InvestmentManager.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly DynamoDBContext _context;

        public ClientRepository(IAmazonDynamoDB dynamoDB)
        {
            _context = new DynamoDBContext(dynamoDB);
        }

        public async Task<Client?> GetClientAsync(string clientId)
            => await _context.LoadAsync<Client>(clientId);

        public async Task UpdateClientAsync(Client client)
            => await _context.SaveAsync(client);
    }
}
