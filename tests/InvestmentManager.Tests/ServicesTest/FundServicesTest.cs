using InvestmentManager.Application.Interfaces;
using InvestmentManager.Application.Services;
using InvestmentManager.Domain.Entities;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentManager.Tests.ServicesTest
{
    public class FundServiceTests
    {
        private readonly Mock<IFundRepository> _fundRepositoryMock;
        private readonly FundService _fundService;

        public FundServiceTests()
        {
            _fundRepositoryMock = new Mock<IFundRepository>();
            _fundService = new FundService(_fundRepositoryMock.Object);
        }

        //GetAllAsync debe devolver fondos correctamente
        [Fact]
        public async Task GetAllAsync_ShouldReturnFunds_WhenFundsExist()
        {
            // Arrange
            var funds = new List<Fund> { new Fund { FundId = 1, Name = "Fondo A" } };
            _fundRepositoryMock.Setup(r => r.GetFundsAsync()).ReturnsAsync(funds);

            // Act
            var result = await _fundService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Fondo A", result.First().Name);
        }

        //GetAllAsync debe lanzar excepción si no hay fondos
        [Fact]
        public async Task GetAllAsync_ShouldThrowException_WhenNoFundsExist()
        {
            // Arrange
            _fundRepositoryMock.Setup(r => r.GetFundsAsync()).ReturnsAsync(new List<Fund>());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _fundService.GetAllAsync());
            Assert.Contains("No se encontraron fondos registrados", exception.Message);
        }

        //GetByIdAsync debe devolver un fondo existente
        [Fact]
        public async Task GetByIdAsync_ShouldReturnFund_WhenFundExists()
        {
            // Arrange
            var fund = new Fund { FundId = 1, Name = "Fondo B" };
            _fundRepositoryMock.Setup(r => r.GetFundByIdAsync(1)).ReturnsAsync(fund);

            // Act
            var result = await _fundService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Fondo B", result.Name);
        }

        //GetByIdAsync debe lanzar excepción si el fondo no existe
        [Fact]
        public async Task GetByIdAsync_ShouldThrowException_WhenFundNotFound()
        {
            // Arrange
            _fundRepositoryMock.Setup(r => r.GetFundByIdAsync(99)).ReturnsAsync((Fund?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _fundService.GetByIdAsync(99));
            Assert.Contains("No se encontró el fondo con ID 99", ex.Message);
        }

        //CreateFundAsync debe crear fondo con ID consecutivo
        [Fact]
        public async Task CreateFundAsync_ShouldCreateFundWithNextId()
        {
            // Arrange
            var existingFunds = new List<Fund>
            {
                new Fund { FundId = 1, Name = "Fondo A" },
                new Fund { FundId = 2, Name = "Fondo B" }
             };

            _fundRepositoryMock.Setup(r => r.GetFundsAsync()).ReturnsAsync(existingFunds);
            _fundRepositoryMock.Setup(r => r.UpdateFundAsync(It.IsAny<Fund>())).Returns(Task.CompletedTask);

            var newFund = new Fund { Name = "Fondo C" };

            // Act
            var result = await _fundService.CreateFundAsync(newFund);

            // Assert
            Assert.Equal(3, result.FundId);
            Assert.Equal("Fondo C", result.Name);
            _fundRepositoryMock.Verify(r => r.UpdateFundAsync(It.IsAny<Fund>()), Times.Once);
        }

        //CreateFundAsync debe lanzar excepción si el nombre es vacío
        [Fact]
        public async Task CreateFundAsync_ShouldThrowException_WhenNameIsEmpty()
        {
            // Arrange
            var fund = new Fund { Name = "" };

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _fundService.CreateFundAsync(fund));
            Assert.Contains("El nombre del fondo es obligatorio.", ex.Message);
        }
    }
}
