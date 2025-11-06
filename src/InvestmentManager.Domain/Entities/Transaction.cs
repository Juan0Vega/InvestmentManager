using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentManager.Domain.Entities
{
    public class Transaction
    {
        public string TransactionId { get; set; } = Guid.NewGuid().ToString();
        public string ClientId { get; set; }
        public string FundId { get; set; }
        public string FundName { get; set; }
        public string Type { get; set; } // "OPEN" | "CANCEL"
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public decimal BalanceAfter { get; set; }
    }

}
