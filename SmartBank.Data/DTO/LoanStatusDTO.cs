using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartBank.Data.Entities;

namespace SmartBank.Data.DTO
{
    public class LoanStatusDTO
    {
        // public int? AccountId { get; set; }



        // public string? LoanType { get; set; }

        // public decimal RequestedAmount { get; set; }

        public decimal? ApprovedAmount { get; set; } 

        public decimal? InterestRate { get; set; }

        // public int TenureMonths { get; set; }

        // public decimal? Emiamount { get; set; }

        // public string? Purpose { get; set; }

        public string? Status { get; set; } // approve by bank

        public DateTime? ReviewedAt { get; set; }

        public string? RejectionReason { get; set; } // done by bank

        // public DateTime? DisbursedAt { get; set; } 

        public DateTime? CreatedAt { get; set; } // when approve by bank then activated

        public DateTime? UpdatedAt { get; set; } // bank will only update it

        // public virtual Account? Account { get; set; }

        // public virtual ICollection<LoanDocument> LoanDocuments { get; set; } = new List<LoanDocument>();

        // public virtual User? ReviewedByUser { get; set; }


    }
}