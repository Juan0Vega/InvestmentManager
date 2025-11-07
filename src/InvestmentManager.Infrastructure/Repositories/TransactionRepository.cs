using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using InvestmentManager.Application.Common;
using InvestmentManager.Application.DTOs;
using InvestmentManager.Application.Interfaces;
using InvestmentManager.Domain.Entities;
using System.Transactions;
using Transaction = InvestmentManager.Domain.Entities.Transaction;

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
        transaction.TransactionId = Guid.NewGuid().ToString();
        await _context.SaveAsync(transaction);
    }

    public async Task<Transaction> CancelSubscription(CancelSubscriptionDTO cancelSubscription)
    {

        var transaction = await _context.LoadAsync<Transaction>(
         cancelSubscription.clientId,
         cancelSubscription.transactionId
         );

        if (transaction == null)
            throw new NotFoundException("Transacción no encontrada ");

        if (transaction.Type == TransactionTypes.CANCEL)
            throw new BusinessException("La transacción ya encuentra cancelada.");

        transaction.Type = TransactionTypes.CANCEL;
        transaction.Timestamp = DateTime.UtcNow;

        await _context.SaveAsync(transaction);
        return transaction;
    }

    public async Task<List<Transaction>> GetTransactionsByClientAsync(int customerId)
    {
        var queryResult = _context.QueryAsync<Transaction>(
          customerId
        );

        return await queryResult.GetRemainingAsync();
    }
}
