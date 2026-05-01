using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartBank.API.services;
using SmartBank.Data.DTO;

namespace SmartBank.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> DepositAsync(DepositwithdrawDTO request)
        {
            if(await _transactionService.DepositAmountAsync(request) is null)
            {
                return BadRequest("Invalid Transaction");
            }

            return Ok(new {amount = request.Amount, status = "success", type = "deposit"});
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> WithdrawAsync(DepositwithdrawDTO request)
        {
            var response = await _transactionService.WithdrawAmountAsync(request);

            if(response is null)
            {
                return BadRequest("Account does not exists: Enter correct accountID");
            }

            if(response.Amount == 0)
            {
                return BadRequest("Insufficient balance");
            }

            return Ok(new {amount = request.Amount, status = "success", type = "withdraw"});
        }

        [HttpPost("transfer-funds")]
        public async Task<IActionResult> TransferAsync(TransferfundsDTO request)
        {
            var response = await _transactionService.TransferFundsAsync(request);

            if(response is null)
            {
                return null;
            }

            return Ok(response);
        }

        [HttpPost("transaction-history")]
        public async Task<IActionResult> GetTransactionHistory(TransactionhistoryDTO request)
        {
            var response = await _transactionService.GetTransactionHistoryAsync(request);

            if(response is null)
            {
                return BadRequest("Cannot fetch the transaction history :: Enter correct account id");
            }

            return Ok(response);
        }

    }
}