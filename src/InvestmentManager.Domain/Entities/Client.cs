using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentManager.Domain.Entities
{
    [DynamoDBTable("Clients")]
    public class Client
    {
        public int ClientId { get; set; } 
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PreferredNotification { get; set; } // "Email" | "SMS"
        public decimal CurrentBalance { get; set; } = 500000; // monto inicial
    }

}
