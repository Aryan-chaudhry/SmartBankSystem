using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartBank.Data.Entities;

namespace SmartBank.Data.DTO
{
    public class ProfileDTO
    {


        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public string? NationalId { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public string? KycStatus { get; set; }

        public bool? IsEmailVerified { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

        public virtual ICollection<KycDocument> KycDocumentUsers { get; set; } = new List<KycDocument>();

        public virtual ICollection<KycDocument> KycDocumentVerifiedByUsers { get; set; } = new List<KycDocument>();

    }
}