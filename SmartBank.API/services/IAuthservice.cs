using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartBank.Data.DTO;

namespace SmartBank.API.services
{
    public interface IAuthService
    {
        Task<RegisteruserDTO?> RegisterAsync (RegisteruserDTO request);
        Task<string> LoginAsync(LoginuserDTO request);
    }
}