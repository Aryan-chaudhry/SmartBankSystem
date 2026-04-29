using System.Security.Cryptography;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartBank.Data.DTO;
using SmartBank.Data.Entities;

namespace SmartBank.API.services
{
    public class AccountService: IAccountService
    {
        private readonly SmartBankContext _context;

        public AccountService(SmartBankContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<decimal>> GetBalanceAsync(GetbalanceDTO request)
        {
            if(request is null) return null;

            var account = await _context.Accounts.FirstOrDefaultAsync(u => u.AccountId == request.AccountId);
            if(account is null) return null;

            return account.Balance;
        }

        public async Task<ActionResult<CreateaccountDTO>> CreateAccountAsync(CreateaccountDTO request)
        {
            try
            {
                if(request is null) return null;
                if(request.AccountType == null)
                {
                    return null;
                }

                // Account can be created call the dbcontext
                Account account = new Account();

                account.UserId = request.UserId;

                account.AccountType = request.AccountType;

                account.AccountNumber =
                    Guid.NewGuid()
                        .ToString()
                        .Replace("-", "")
                        .Substring(0, 20); // MUST be <= 20

                account.Balance = 0;

                account.Currency = "INR";

                account.Status = "Active";

                account.MinimumBalance =
                    request.AccountType == "Savings"
                        ? 1000
                        : 5000;

                account.InterestRate =
                    request.AccountType == "Savings"
                        ? 3.5m
                        : 0m;

                account.BranchCode = "DEL01";

                account.Ifsccode = "SBIN0001234";

                account.OpenedAt = DateTime.UtcNow;

                account.UpdatedAt = DateTime.UtcNow;

                _context.Accounts.Add(account);

                var response = await _context.SaveChangesAsync();
                Console.WriteLine(response);
            }
            catch(Exception ex)
            {
                Console.WriteLine("MAIN ERROR❌");
                Console.WriteLine(ex.Message);
            }
            

            return request;
        }

        public async Task<ActionResult<string?>> GetAccountStatusAsyc(GetaccountstatusDTO request)
        {
            if(request is null)
            {
                return null;
            }

            var account = await _context.Accounts.FirstOrDefaultAsync(u => u.AccountId == request.AccountId);

            if(account is null)
            {
                return null;
            }

            return account.Status;
        }
    }
}