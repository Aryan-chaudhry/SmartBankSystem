using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SmartBank.Data.Entities;

namespace SmartBank.Data.DTO
{
    public class UploadKYCDTO
    {
        // public int KycDocumentId { get; set; }



        public string? DocumentType { get; set; }

        public string DocumentNumber { get; set; } = null!;

        public string FilePath { get; set; } = null!;

        public DateTime? UploadedAt { get; set; }



        // public virtual User User { get; set; } = null!;

        // public virtual User? VerifiedByUser { get; set; }
    }
}