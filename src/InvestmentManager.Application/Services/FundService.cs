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
            return await _fundRepository.GetFundsAsync();
        }

        public async Task<Fund?> GetByIdAsync(string fundId)
        {
            return await _fundRepository.GetFundByIdAsync(fundId);
        }
    }
}
