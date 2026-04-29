using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartBank.Data.DTO
{
    public class RegisteruserDTO
    {   

        public int RoleId { get; set; }

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public string? NationalId {get; set;}

        public DateTime? CreatedAt { get; set; }

        

    }

}
