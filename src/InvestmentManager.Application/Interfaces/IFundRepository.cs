using InvestmentManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentManager.Application.Interfaces
{
    public interface IFundRepository
    {
        Task<List<Fund>> GetFundsAsync();
        Task<Fund?> GetFundByIdAsync(string fundId);
    }
}
