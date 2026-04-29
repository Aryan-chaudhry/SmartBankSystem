using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBank.Data.DTO
{
    public class RaiseTicketDTO
    {

 
        public int? AssignedToUserId { get; set; }

        public string Subject { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? Category { get; set; }

        //  public string? Priority { get; set; }

        // public string? Status { get; set; };

        // public string? Resolution { get; set; }

        public DateTime? CreatedAt { get; set; }

        // public DateTime? UpdatedAt { get; set; }

        // public DateTime? ResolvedAt { get; set; }

    }
}