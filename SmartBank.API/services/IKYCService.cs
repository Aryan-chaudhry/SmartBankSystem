using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartBank.Data.DTO;

namespace SmartBank.API.services
{
    public interface IKYCService
    {
        Task<UploadKYCDTO?> UploadKYCAsync (UploadKYCDTO kyc, string Email);
    }
}