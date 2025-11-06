using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using InvestmentManager.Application.Interfaces;
using InvestmentManager.Domain.Entities;

namespace InvestmentManager.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly DynamoDBContext _context;

    public TransactionRepository(IAmazonDynamoDB dynamoDB)
    {
        _context = new DynamoDBContext(dynamoDB);
    }

    public async Task AddTransactionAsync(Transaction transaction)
    {
        await _context.SaveAsync(transaction);
    }

    public async Task<List<Transaction>> GetTransactionsByClientAsync(string customerId)
    {
        var scanConditions = new List<ScanCondition>
        {
            new ScanCondition("ClientId", ScanOperator.Equal, customerId)
        };

        return await _context.ScanAsync<Transaction>(scanConditions).GetRemainingAsync();
    }
}
