using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentManager.Application.DTOs
{
    public class CancelSubscriptionDTO
    {
        public int clientId { get; set; }
        public string transactionId { get; set; }

    }
}
