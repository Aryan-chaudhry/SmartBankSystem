using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SmartBank.Data.DTO;
using SmartBank.Data.Entities;

namespace SmartBank.API.services
{
    public class TransactionService: ITransactionService
    {
        private readonly SmartBankContext _context;

        public TransactionService(SmartBankContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetTransactionHistoryAsync(TransactionhistoryDTO request)
        {
            var transactions =
                await _context.Transactions
                    .Where(t => t.AccountId == request.AccountId)
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync();


            return transactions;
        }

        public async Task<DepositwithdrawDTO> WithdrawAmountAsync(DepositwithdrawDTO request)
        {
            var account = await _context.Accounts.FindAsync(request.AccountId);

            if(account is null)
            {
                return null;
            }

            if(account.Balance < request.Amount)
            {
                request.Amount = 0;
                return request;
            }

            // we can withdraw
            account.Balance -= request.Amount;

            var transaction = new Transaction();

            transaction.Account = account;
            transaction.AccountId = account.AccountId;
            transaction.Amount = request.Amount;
            transaction.CreatedAt = DateTime.UtcNow;
            transaction.BalanceAfter = account.Balance;
            transaction.Description = $"Withdraw {request.Amount} from account";
            transaction.Channel = "ATM";
            transaction.ReferenceNumber = Guid.NewGuid().ToString();
            transaction.Status = "Success";
            transaction.PerformedByUserId = account.UserId;

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return request;
        }

        public async Task<DepositwithdrawDTO> DepositAmountAsync(DepositwithdrawDTO request)
        {
            var account = await _context.Accounts.FindAsync(request.AccountId);

            if(account is null)
            {
                return null;
            }

            account.Balance += request.Amount;
            var transaction = new Transaction();

            transaction.Account = account;
            transaction.AccountId = account.AccountId;
            transaction.Amount = request.Amount;
            transaction.CreatedAt = DateTime.UtcNow;
            transaction.TransactionType = "Deposit";
            transaction.BalanceAfter = account.Balance;
            transaction.Description = $"Deposited {request.Amount} in account";
            transaction.Channel = "ATM";
            transaction.ReferenceNumber = Guid.NewGuid().ToString();
            transaction.Status = "Success";
            transaction.PerformedByUserId = account.UserId;

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return request;
        }

        public async Task<TransferfundsDTO> TransferFundsAsync(TransferfundsDTO request)
        {
            if (request is null)
                return null;

            // Start DB transaction
            using var dbTransaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var account_sender =
                    await _context.Accounts
                        .FindAsync(request.FromAccountId);

                var account_receiver =
                    await _context.Accounts
                        .FindAsync(request.ToAccountId);

                if (account_sender is null ||
                    account_receiver is null)
                {
                    return null;
                }

                var user_sender =
                    await _context.Users
                        .FirstOrDefaultAsync(u =>
                            u.UserId == account_sender.UserId);

                var user_receiver =
                    await _context.Users
                        .FirstOrDefaultAsync(u =>
                            u.UserId == account_receiver.UserId);

                if (user_sender is null ||
                    user_receiver is null)
                {
                    return null;
                }

                // Prevent same account transfer
                if (account_sender.AccountId ==
                    account_receiver.AccountId)
                {
                    return null;
                }

                // Check balance
                if (account_sender.Balance < request.Amount)
                {
                    return null;
                }

                // Update balances
                account_sender.Balance -= request.Amount;
                account_receiver.Balance += request.Amount;

                // Create transfer
                var transfer = new Transfer
                {
                    FromAccountId = account_sender.AccountId,
                    ToAccountId = account_receiver.AccountId,
                    Amount = request.Amount,
                    CreatedAt = DateTime.UtcNow,
                    Status = "Success",
                    ReferenceNumber = Guid.NewGuid().ToString(),

                    InitiatedByUserId = user_sender.UserId
                };

                await _context.Transfers.AddAsync(transfer);

                // Save to generate TransferId
                await _context.SaveChangesAsync();

                // Sender transaction
                var transaction_sender = new Transaction
                {
                    AccountId = account_sender.AccountId,
                    TransferId = transfer.TransferId,
                    Amount = request.Amount,
                    CreatedAt = DateTime.UtcNow,
                    Description =
                        $"Transferred {request.Amount} to account {account_receiver.AccountNumber}",
                    BalanceAfter = account_sender.Balance,
                    PerformedByUserId = user_sender.UserId,
                    Status = "Success",
                    TransactionType = "Debit",
                    Channel = "UPI",
                    ReferenceNumber = Guid.NewGuid().ToString()
                };

                // Receiver transaction
                var transaction_receiver = new Transaction
                {
                    AccountId = account_receiver.AccountId,
                    TransferId = transfer.TransferId,
                    Amount = request.Amount,
                    CreatedAt = DateTime.UtcNow,
                    Description =
                        $"Received {request.Amount} from account {account_sender.AccountNumber}",
                    BalanceAfter = account_receiver.Balance,

                    ReferenceNumber = Guid.NewGuid().ToString(),
                    PerformedByUserId = user_receiver.UserId,

                    Status = "Success",
                    TransactionType = "Credit",
                    Channel = "UPI"
                };

                await _context.Transactions
                    .AddAsync(transaction_sender);

                await _context.Transactions
                    .AddAsync(transaction_receiver);

                await _context.SaveChangesAsync();

                await dbTransaction.CommitAsync();

                return request;
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
        }
    }
}