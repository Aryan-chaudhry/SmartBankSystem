using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartBank.Data.DTO;
using SmartBank.Data.Entities;

namespace SmartBank.API.services
{
    public interface ITransactionService
    {
        Task<DepositwithdrawDTO> DepositAmountAsync(DepositwithdrawDTO request);

        Task<DepositwithdrawDTO> WithdrawAmountAsync(DepositwithdrawDTO request);

        Task<TransferfundsDTO> TransferFundsAsync(TransferfundsDTO request);

        Task<List<Transaction>> GetTransactionHistoryAsync(TransactionhistoryDTO request);
    }
}