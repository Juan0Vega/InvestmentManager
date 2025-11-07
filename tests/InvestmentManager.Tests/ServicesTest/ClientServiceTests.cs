using InvestmentManager.Application.Common;
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
    public class ClientServiceTests
    {
        private readonly Mock<IClientRepository> _mockRepo;
        private readonly ClientService _clientService;

        public ClientServiceTests()
        {
            _mockRepo = new Mock<IClientRepository>();
            _clientService = new ClientService(_mockRepo.Object);
        }

        //GetByIdAsync - Cliente encontrado
        [Fact]
        public async Task GetByIdAsync_ShouldReturnClient_WhenClientExists()
        {
            // Arrange
            var client = new Client { ClientId = 1, Name = "Juan" };
            _mockRepo.Setup(r => r.GetClientAsync(1)).ReturnsAsync(client);

            // Act
            var result = await _clientService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Juan", result.Name);
        }

        //GetByIdAsync - Cliente no encontrado
        [Fact]
        public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenClientDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetClientAsync(It.IsAny<int>())).ReturnsAsync((Client?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => _clientService.GetByIdAsync(99));
            Assert.Contains("No se encontró el cliente con ID 99", ex.Message);
        }

        //CreateAsync - Cliente creado exitosamente
        [Fact]
        public async Task CreateAsync_ShouldCreateClient_WhenValidData()
        {
            // Arrange
            var newClient = new Client { ClientId = 10, Name = "Maria" };
            _mockRepo.Setup(r => r.GetClientAsync(newClient.ClientId)).ReturnsAsync((Client?)null);
            _mockRepo.Setup(r => r.UpdateClientAsync(newClient)).Returns(Task.CompletedTask);

            // Act
            await _clientService.CreateAsync(newClient);

            // Assert
            _mockRepo.Verify(r => r.UpdateClientAsync(It.Is<Client>(c => c.ClientId == 10 && c.CurrentBalance == ClientInitialAmount.InitialAmount)), Times.Once);
        }

        //CreateAsync - Cliente nulo
        [Fact]
        public async Task CreateAsync_ShouldThrowBusinessException_WhenClientIsNull()
        {
            // Act & Assert
            var ex = await Assert.ThrowsAsync<BusinessException>(() => _clientService.CreateAsync(null));
            Assert.Equal("Los datos del cliente son inválidos.", ex.Message);
        }

        //CreateAsync - Cliente duplicado
        [Fact]
        public async Task CreateAsync_ShouldThrowBusinessException_WhenClientAlreadyExists()
        {
            // Arrange
            var existingClient = new Client { ClientId = 1, Name = "Juan" };
            _mockRepo.Setup(r => r.GetClientAsync(existingClient.ClientId)).ReturnsAsync(existingClient);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<BusinessException>(() => _clientService.CreateAsync(existingClient));
            Assert.Contains($"Ya existe un cliente con el ID {existingClient.ClientId}", ex.Message);
        }

        //GetAllClients - Devuelve lista de clientes
        [Fact]
        public async Task GetAllClients_ShouldReturnClientsList()
        {
            // Arrange
            var clients = new List<Client>
            {
                new Client { ClientId = 1, Name = "Juan" },
                new Client { ClientId = 2, Name = "Maria" }
            };
            _mockRepo.Setup(r => r.GetAllClients()).ReturnsAsync(clients);

            // Act
            var result = await _clientService.GetAllClients();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.Name == "Maria");
        }
    }
}
