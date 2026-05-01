using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartBank.Data.Entities;

namespace SmartBank.API.services
{
    public interface IReportService
    {
        Task<List<Transaction>> GetDailyTransactionReport();

        Task<List<Loan>> GetApprovedLoans();

        Task<List<User>> GetActiveUsers();
    }
}