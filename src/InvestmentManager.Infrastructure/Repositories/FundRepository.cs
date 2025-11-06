using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using InvestmentManager.Application.Interfaces;
using InvestmentManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentManager.Infrastructure.Repositories
{
    public class FundRepository : IFundRepository
    {
        private readonly DynamoDBContext _context;
        public FundRepository(IAmazonDynamoDB dynamoDB)
        {
            _context = new DynamoDBContext(dynamoDB);
        }
        public async Task<Fund?> GetFundByIdAsync(string fundId)
        {
            return await _context.LoadAsync<Fund>(fundId);
        }

        public async Task<List<Fund>> GetFundsAsync()
        {
            // DynamoDB no usa "SELECT *", se hace un Scan sin condiciones.
            var scanConditions = new List<ScanCondition>();
            return await _context.ScanAsync<Fund>(scanConditions).GetRemainingAsync();
        }

      
    }
}
