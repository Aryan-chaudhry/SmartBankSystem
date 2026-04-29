using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartBank.Data.Entities;

namespace SmartBank.Data.DTO
{
    public class UploadLoanDocumentDTO
    {
        // public int LoanDocumentId { get; set; }

        // public int LoanId { get; set; }

        public string? DocumentType { get; set; }

        public string FilePath { get; set; } = null!;

        public DateTime? UploadedAt { get; set; }

        // public virtual Loan Loan { get; set; } = null!;
    }
}