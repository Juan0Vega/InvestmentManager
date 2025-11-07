using InvestmentManager.Application.Interfaces;
using InvestmentManager.Domain.Entities;

namespace InvestmentManager.Application.Services
{
    public class FundService : IFundService
    {
        private readonly IFundRepository _fundRepository;

        public FundService(IFundRepository fundRepository)
        {
            _fundRepository = fundRepository;
        }

        public async Task<IEnumerable<Fund>> GetAllAsync()
        {
            try
            {
                var funds = await _fundRepository.GetFundsAsync();

                if (funds == null || !funds.Any())
                    throw new Exception("No se encontraron fondos registrados.");

                return funds;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener los fondos: {ex.Message}");
            }
        }

        public async Task<Fund?> GetByIdAsync(int fundId)
        {
            try
            {
                if (fundId == null)
                    throw new ArgumentException("El ID del fondo no puede estar vacío.");

                var fund = await _fundRepository.GetFundByIdAsync(fundId);

                if (fund == null)
                    throw new Exception($"No se encontró el fondo con ID {fundId}.");

                return fund;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar el fondo con ID {fundId}: {ex.Message}");
            }
        }

        public async Task<Fund> CreateFundAsync(Fund fund)
        {
            try
            {
                if (fund == null)
                    throw new ArgumentNullException(nameof(fund), "Los datos del fondo son inválidos.");

                if (string.IsNullOrWhiteSpace(fund.Name))
                    throw new ArgumentException("El nombre del fondo es obligatorio.");

                var funds = await _fundRepository.GetFundsAsync();
                int nextId = 1;

                if (funds.Any())
                    nextId = funds.Max(f => f.FundId) + 1;

                fund.FundId = nextId;

                await _fundRepository.UpdateFundAsync(fund);

                return fund;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear el fondo: {ex.Message}");
            }
        }
    }
}
