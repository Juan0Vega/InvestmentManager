using Amazon.DynamoDBv2.DataModel;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InvestmentManager.Domain.Entities
{
    [DynamoDBTable("Transactions")]
    public class Transaction
    {
        public string TransactionId { get; set; } = Guid.NewGuid().ToString();
        public int ClientId { get; set; }
        public int FundId { get; set; }
        public string FundName { get; set; }

        [DynamoDBProperty]
        public TransactionTypes Type { get; set; } // "OPEN" | "CANCEL"
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public decimal BalanceAfter { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransactionTypes
    {
        OPEN,
        CANCEL
    }

}
