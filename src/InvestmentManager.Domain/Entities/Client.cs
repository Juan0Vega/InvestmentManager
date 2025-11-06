using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentManager.Domain.Entities
{
    public class Client
    {
        public string ClientId { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PreferredNotification { get; set; } // "Email" | "SMS"
        public decimal CurrentBalance { get; set; } = 500000; // monto inicial
    }

}
