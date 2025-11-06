using InvestmentManager.Domain.Entities;

namespace InvestmentManager.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<List<Transaction?>> GetByIdAsync(string clientId);
        Task CreateAsync(Transaction transaction);
    }
}
