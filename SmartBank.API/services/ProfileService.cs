using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using SmartBank.Data.DTO;
using SmartBank.Data.Entities;

namespace SmartBank.API.services
{
    public class ProfileService : IProfileService
    {
        private readonly SmartBankContext _context;

        public ProfileService(SmartBankContext context)
        {
            _context = context; 
        }

        public async Task<ProfileDTO?> GetProfileAsync(string EmailId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == EmailId);

            var profile = new ProfileDTO
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                NationalId = user.NationalId,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Address = user.Address,
                City = user.City,
                Country = user.Country,
                KycStatus = "Pending",
                IsEmailVerified = false,
                IsActive = true,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Accounts = user.Accounts,
                KycDocumentUsers = user.KycDocumentUsers,
                KycDocumentVerifiedByUsers = user.KycDocumentVerifiedByUsers

            };

            return profile;
        }

        public async Task<EditProfileDTO> EditProfileAsync(EditProfileDTO profile)
        {
            var User = await _context.Users.FirstOrDefaultAsync(u => u.Email == profile.Email);

            User.FirstName = profile.FirstName;
            User.LastName = profile.LastName;
            User.PhoneNumber = profile.PhoneNumber;
            User.DateOfBirth = profile.DateOfBirth;
            User.Gender = profile.Gender;
            User.Address = profile.Address;
            User.City = profile.City;
            User.Country = profile.Country;
            User.UpdatedAt = DateTime.UtcNow;


             _context.Users.Update(User);
             await _context.SaveChangesAsync();

             return profile;
        }
    }
}