using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBank.Data.DTO
{
    public class DepositwithdrawDTO
    {

        public int AccountId { get; set; }

        public string? TransactionType { get; set; }

        public decimal Amount { get; set; }

    }
}