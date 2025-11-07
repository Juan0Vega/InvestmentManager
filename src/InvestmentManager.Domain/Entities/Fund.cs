using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestmentManager.Domain.Entities
{
    [DynamoDBTable("Funds")]
    public class Fund
    {
        public int FundId { get; set; }
        public string Name { get; set; }
        public decimal MinAmount { get; set; }
        public string Category { get; set; }
    }

}
