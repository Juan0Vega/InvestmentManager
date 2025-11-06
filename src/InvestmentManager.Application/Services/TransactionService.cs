using InvestmentManager.Application.Interfaces;
using InvestmentManager.Domain.Entities;

namespace InvestmentManager.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<List<Transaction?>> GetByIdAsync(string clientId)
        {
            return await _transactionRepository.GetTransactionsByClientAsync(clientId);
        }

        public async Task CreateAsync(Transaction transaction)
        {
            await _transactionRepository.AddTransactionAsync(transaction);
        }
    }
}
