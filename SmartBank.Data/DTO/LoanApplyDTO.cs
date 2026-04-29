using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartBank.Data.Entities;

namespace SmartBank.Data.DTO
{
    public class LoanApplyDTO
    {

        // public int? AccountId { get; set; }

        public string? LoanType { get; set; }

        public decimal RequestedAmount { get; set; }

        public int TenureMonths { get; set; }

        public decimal? Emiamount { get; set; }

        public string? Purpose { get; set; }

        public DateTime? DisbursedAt { get; set; } 

        
    }
}