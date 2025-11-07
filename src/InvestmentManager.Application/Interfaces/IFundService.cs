using InvestmentManager.Domain.Entities;

namespace InvestmentManager.Application.Interfaces
{
    public interface IFundService
    {
        Task<IEnumerable<Fund>> GetAllAsync();
        Task<Fund?> GetByIdAsync(int fundId);
        Task<Fund> CreateFundAsync(Fund fund);
    }
}
