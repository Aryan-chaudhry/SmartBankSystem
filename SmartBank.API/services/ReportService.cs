using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartBank.Data.Entities;

namespace SmartBank.API.services
{
    public class ReportService: IReportService
    {
        private readonly SmartBankContext _context;

        public ReportService(SmartBankContext context)
        {
            _context = context;
        }
        public async Task<List<Transaction>> GetDailyTransactionReport()
        {
            var today = DateTime.UtcNow.Date;

            var report =
                await _context.Transactions
                    .Where(t => t.CreatedAt == today).ToListAsync();

            return report;
        }

        public async Task<List<Loan>> GetApprovedLoans()
        {
            var loans =
                await _context.Loans
                    .Where(l => l.Status == "Approved")
                    .OrderByDescending(l => l.ReviewedAt)
                    .ToListAsync();
            
            return loans;
        }

        public async Task<List<User>> GetActiveUsers()
        {
            var active_users = await _context.Users.Where(
                u => u.IsActive == true && u.IsFrozen == false
            ).ToListAsync();

            return active_users;
        }
    }
}