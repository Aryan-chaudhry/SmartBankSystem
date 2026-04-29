using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartBank.Data.DTO;

namespace SmartBank.API.services
{
    public interface IProfileService
    {
        Task<ProfileDTO> GetProfileAsync(string EmailId);

        Task<EditProfileDTO> EditProfileAsync(EditProfileDTO profile);

        
    }
}