using Amazon.Runtime.Internal;
using InvestmentManager.Application.DTOs;
using InvestmentManager.Application.Interfaces;
using InvestmentManager.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Transactions;
using Transaction = InvestmentManager.Domain.Entities.Transaction;

namespace InvestmentManager.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IFundRepository _fundRepository;
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(
            IClientRepository clientRepository,
            IFundRepository fundRepository,
            ITransactionRepository transactionRepository)
        {
            _clientRepository = clientRepository;
            _fundRepository = fundRepository;
            _transactionRepository = transactionRepository;
        }

        public async Task<List<Transaction?>> GetTransationsByIdAsync(int clientId)
        {
            return await _transactionRepository.GetTransactionsByClientAsync(clientId);
        }

        public async Task<string> CreateAsync(Transaction transaction)
        {
            // Validacion del cliente
            
            var client = await _clientRepository.GetClientAsync(transaction.ClientId);
            if (client == null)
                throw new Exception($"El cliente {transaction.ClientId} no existe.");

            // Validacion saldo disponible
            if (client.CurrentBalance <= 0)
                throw new Exception($"El cliente {transaction.ClientId} no tiene saldo disponible.");

            // Validacion del fondo
            var fund = await _fundRepository.GetFundByIdAsync(transaction.FundId);
            if (fund == null)
                throw new Exception($"El fondo {transaction.FundId} no existe.");

            // Validacion monto minimo del fondo
            if (fund.MinAmount > client.CurrentBalance)
                throw new Exception($"No tiene saldo disponible para vincularse al fondo {fund.Name}.");

            // Validacion monto de transacción
            if (transaction.Amount > client.CurrentBalance)
                throw new Exception($"El monto ({transaction.Amount}) no puede ser mayor al saldo actual del cliente ({client.CurrentBalance}).");

            if(fund.MinAmount > transaction.Amount)
                throw new Exception($"El monto minimo para invertir al fondo es de: ({fund.MinAmount}).");

            // Parametros de transacción
            transaction.FundName = fund.Name;
            transaction.Type = TransactionTypes.OPEN;
            transaction.Timestamp = DateTime.UtcNow;
            transaction.BalanceAfter = client.CurrentBalance - transaction.Amount;

            // Guardar transacción
            await _transactionRepository.AddTransactionAsync(transaction);

            // Actualizar saldo del cliente
            client.CurrentBalance -= transaction.Amount;
            await _clientRepository.UpdateClientAsync(client);

            return transaction.TransactionId;
        }

        public async Task<Transaction> CancelSubscription(CancelSubscriptionDTO cancelSubscription)
        {
            var cancelTransaction = await _transactionRepository.CancelSubscription(cancelSubscription);
            var client = await _clientRepository.GetClientAsync(cancelSubscription.clientId);

            if (client == null)
                throw new Exception($"El cliente {cancelSubscription.clientId} no existe.");

            client.CurrentBalance += cancelTransaction.Amount;
            await _clientRepository.UpdateClientAsync(client);

            return cancelTransaction;
        }
    }
}
