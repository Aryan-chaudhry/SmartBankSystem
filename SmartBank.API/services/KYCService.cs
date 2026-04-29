using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartBank.Data.DTO;
using SmartBank.Data.Entities;

namespace SmartBank.API.services
{
    public class KYCService : IKYCService
    {
        private readonly SmartBankContext _context;

        public KYCService(SmartBankContext context)
        {
            this._context = context;
        }

        public async Task<UploadKYCDTO?> UploadKYCAsync (UploadKYCDTO kyc, string Email)
        {
            if(kyc is null || Email is null)
            {
                return null;
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);

            if(user is null)
            {
                return null;
            }

            var KycDocument = new KycDocument();

            KycDocument.UserId = user.UserId;
            KycDocument.DocumentType = kyc.DocumentType;
            KycDocument.DocumentNumber = kyc.DocumentNumber;
            KycDocument.FilePath = kyc.FilePath;
            KycDocument.UploadedAt = DateTime.UtcNow;

            await _context.KycDocuments.AddAsync(KycDocument);
            user.KycDocumentUsers.Add(KycDocument);


            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return kyc;
        }
    }
}