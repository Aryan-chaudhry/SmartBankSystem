using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBank.Data.DTO
{
    public class TransferfundsDTO
    {
        public int FromAccountId { get; set; }

        public int ToAccountId { get; set; }

        public decimal Amount { get; set; }

        public int InitiatedByUserId { get; set; }
    }
}