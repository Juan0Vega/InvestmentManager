using InvestmentManager.Application.DTOs;
using InvestmentManager.Application.Interfaces;
using InvestmentManager.Application.Services;
using InvestmentManager.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentManager.Tests.ServicesTest
{
    public class TransactionServiceTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IClientRepository> _clientRepositoryMock;
        private readonly Mock<IFundRepository> _fundRepositoryMock;
        private readonly TransactionService _transactionService;

        public TransactionServiceTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _clientRepositoryMock = new Mock<IClientRepository>();
            _fundRepositoryMock = new Mock<IFundRepository>();

            _transactionService = new TransactionService(
                _clientRepositoryMock.Object,
                _fundRepositoryMock.Object,
                _transactionRepositoryMock.Object
            );
        }

    
        [Fact]
        public async Task GetTransationsByIdAsync_ShouldReturnTransactions_WhenClientExists()
        {
            // Arrange
            var clientId = 1;
            var transactions = new List<Transaction?>
            {
                new Transaction { TransactionId = "T1", ClientId = clientId, Amount = 100 },
                new Transaction { TransactionId = "T2", ClientId = clientId, Amount = 200 }
            };

            _transactionRepositoryMock
                .Setup(r => r.GetTransactionsByClientAsync(clientId))
                .ReturnsAsync(transactions);

            // Act
            var result = await _transactionService.GetTransationsByIdAsync(clientId);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("T1", result[0]?.TransactionId);
            _transactionRepositoryMock.Verify(r => r.GetTransactionsByClientAsync(clientId), Times.Once);
        }


        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenClientDoesNotExist()
        {
            // Arrange
            var transaction = new Transaction { ClientId = 99, FundId = 1, Amount = 100 };
            _clientRepositoryMock.Setup(r => r.GetClientAsync(transaction.ClientId))
                                 .ReturnsAsync((Client?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _transactionService.CreateAsync(transaction));
            Assert.Contains("no existe", ex.Message);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenClientHasNoBalance()
        {
            // Arrange
            var transaction = new Transaction { ClientId = 1, FundId = 1, Amount = 100 };
            var client = new Client { ClientId = 1, CurrentBalance = 0 };

            _clientRepositoryMock.Setup(r => r.GetClientAsync(1))
                                 .ReturnsAsync(client);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _transactionService.CreateAsync(transaction));
            Assert.Contains("no tiene saldo disponible", ex.Message);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenFundDoesNotExist()
        {
            // Arrange
            var transaction = new Transaction { ClientId = 1, FundId = 1, Amount = 100 };
            var client = new Client { ClientId = 1, CurrentBalance = 500 };

            _clientRepositoryMock.Setup(r => r.GetClientAsync(1))
                                 .ReturnsAsync(client);

            _fundRepositoryMock.Setup(r => r.GetFundByIdAsync(1))
                               .ReturnsAsync((Fund?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _transactionService.CreateAsync(transaction));
            Assert.Contains("no existe", ex.Message);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateTransaction_WhenDataIsValid()
        {
            // Arrange
            var transaction = new Transaction
            {
                TransactionId = "T100",
                ClientId = 1,
                FundId = 1,
                Amount = 100
            };

            var client = new Client { ClientId = 1, CurrentBalance = 500 };
            var fund = new Fund { FundId = 1, Name = "Fondo A", MinAmount = 50 };

            _clientRepositoryMock.Setup(r => r.GetClientAsync(1)).ReturnsAsync(client);
            _fundRepositoryMock.Setup(r => r.GetFundByIdAsync(1)).ReturnsAsync(fund);

            // Act
            var result = await _transactionService.CreateAsync(transaction);

            // Assert
            Assert.Equal("T100", result);
            _transactionRepositoryMock.Verify(r => r.AddTransactionAsync(It.IsAny<Transaction>()), Times.Once);
            _clientRepositoryMock.Verify(r => r.UpdateClientAsync(It.IsAny<Client>()), Times.Once);
        }

        [Fact]
        public async Task CancelSubscription_ShouldThrowException_WhenClientDoesNotExist()
        {
            // Arrange
            var dto = new CancelSubscriptionDTO { clientId = 1, transactionId = "10" };
            var transaction = new Transaction { Amount = 100 };

            _transactionRepositoryMock.Setup(r => r.CancelSubscription(dto))
                                      .ReturnsAsync(transaction);

            _clientRepositoryMock.Setup(r => r.GetClientAsync(dto.clientId))
                                 .ReturnsAsync((Client?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _transactionService.CancelSubscription(dto));
            Assert.Contains("no existe", ex.Message);
        }

        [Fact]
        public async Task CancelSubscription_ShouldUpdateClientBalance_WhenValid()
        {
            // Arrange
            var dto = new CancelSubscriptionDTO { clientId = 1, transactionId = "10" };
            var transaction = new Transaction { Amount = 100 };
            var client = new Client { ClientId = 1, CurrentBalance = 500 };

            _transactionRepositoryMock.Setup(r => r.CancelSubscription(dto))
                                      .ReturnsAsync(transaction);

            _clientRepositoryMock.Setup(r => r.GetClientAsync(dto.clientId))
                                 .ReturnsAsync(client);

            // Act
            var result = await _transactionService.CancelSubscription(dto);

            // Assert
            Assert.Equal(600, client.CurrentBalance);
            Assert.Equal(transaction, result);
            _transactionRepositoryMock.Verify(r => r.CancelSubscription(dto), Times.Once);
            _clientRepositoryMock.Verify(r => r.UpdateClientAsync(client), Times.Once);
        }
    }
}
