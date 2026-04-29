using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartBank.Data.Entities;

namespace SmartBank.Data.DTO
{
    public class EditProfileDTO
    {


        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? PhoneNumber { get; set; }


        public DateOnly? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public DateTime? UpdatedAt { get; set; }

    }
}