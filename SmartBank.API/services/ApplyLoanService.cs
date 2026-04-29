using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartBank.Data.DTO;
using SmartBank.Data.Entities;

namespace SmartBank.API.services
{
    public class ApplyLoanService : IApplyLoanService
    {
        private readonly SmartBankContext _context;

        public ApplyLoanService(SmartBankContext context)
        {
            this._context = context;
        }

        public async Task<LoanApplyDTO?> ApplyLoanAsyn(LoanApplyDTO loan, string Email)
        {
            if(loan is null || Email is null)
            {
                return null;
            }
            
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);

            if(user is null)
            {
                return null;
            }

            var Account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == user.UserId);
            


            if(Account is null)
            {
                return null;
            }

            var Loan = new Loan();

            Loan.UserId = user.UserId;
            Loan.AccountId = Account.AccountId;
            Loan.LoanType = loan.LoanType;
            Loan.RequestedAmount = loan.RequestedAmount;
            Loan.TenureMonths = loan.TenureMonths;
            Loan.Emiamount = loan.Emiamount;
            Loan.Purpose = loan.Purpose;
            Loan.DisbursedAt = loan.DisbursedAt;
           

            await _context.Loans.AddAsync(Loan);
            await _context.SaveChangesAsync();
            return loan;
        }

        public async Task<UploadLoanDocumentDTO?> UploadLoanDocsAsync(UploadLoanDocumentDTO docs, string Email)
        {
            if(docs is null || Email is null )
            {
                return null;
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);

            if(user is null)
            {
                return null;
            }

            var loan = await _context.Loans.FirstOrDefaultAsync(l => l.UserId == user.UserId);

            if(loan is null)
            {
                return null;
            }

            var loanDocs = new LoanDocument();

            loanDocs.LoanId = loan.LoanId;
            loanDocs.DocumentType = docs.DocumentType;
            loanDocs.FilePath = docs.FilePath;
            loanDocs.UploadedAt = DateTime.UtcNow;
            loanDocs.Loan = loan;

            await _context.LoanDocuments.AddAsync(loanDocs);
            await _context.SaveChangesAsync();
            return docs;
        }

        public async Task<LoanStatusDTO?> LoanStatusAsync (LoanStatusDTO loanApplied, string Email)
        {
            if(loanApplied is null || Email is null)
            {
                return null;
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);

            if(user is null)
            {
                return null;
            }

            var loan = await _context.Loans.FirstOrDefaultAsync(l => l.UserId == user.UserId);

            if(loan is null)
            {
                return null;
            }

            loan.ApprovedAmount = loanApplied.ApprovedAmount;
            loan.InterestRate = loanApplied.InterestRate;
            loan.Status = loanApplied.Status;
            loan.ReviewedAt = loanApplied.ReviewedAt;
            loan.RejectionReason = loanApplied.RejectionReason;
            loan.CreatedAt = loanApplied.CreatedAt;
            loan.UpdatedAt = DateTime.UtcNow;
            loan.User = user;


            await _context.SaveChangesAsync();
            return loanApplied;
        }
    }
}