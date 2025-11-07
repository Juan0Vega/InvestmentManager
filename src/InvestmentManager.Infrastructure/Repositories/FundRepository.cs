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
        public async Task<Fund?> GetFundByIdAsync(int fundId)
            => await _context.LoadAsync<Fund>(fundId);
        

        public async Task<List<Fund>> GetFundsAsync()
        {
            
            var scanConditions = new List<ScanCondition>();
            return await _context.ScanAsync<Fund>(scanConditions).GetRemainingAsync();
        }

        public async Task UpdateFundAsync(Fund fund)
            => await _context.SaveAsync(fund);
        
    }
}   
