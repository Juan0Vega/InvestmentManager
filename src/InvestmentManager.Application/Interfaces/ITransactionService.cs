using InvestmentManager.Application.DTOs;
using InvestmentManager.Domain.Entities;

namespace InvestmentManager.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<List<Transaction?>> GetTransationsByIdAsync(int clientId);
        Task<string>CreateAsync(Transaction transaction);

        Task<Transaction> CancelSubscription(CancelSubscriptionDTO cancelSubscription); 
    }
}
