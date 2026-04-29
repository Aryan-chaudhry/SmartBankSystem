using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartBank.API.services;
using SmartBank.Data.DTO;

namespace SmartBank.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountservice)
        {
            _accountService = accountservice;
        }

        [HttpPost("status")]
        public async Task<IActionResult> GetAccountStatus(GetaccountstatusDTO request)
        {
            var response = await _accountService.GetAccountStatusAsyc(request);

            if(response is null)
            {
                return BadRequest("Account id does not exists");
            }

            return Ok(new { status = response.Value });
        }

        [HttpPost("balance")]
        public async Task<IActionResult> GetBalance(GetbalanceDTO request)
        {
            var response = await _accountService.GetBalanceAsync(request);
            Console.WriteLine("Response: " + response);
            if(response is null)
            {
                return BadRequest("Account does not exists");
            }

            return Ok(new { balance = response.Value });
        }

        [HttpPost("create")]
        public async Task<ActionResult<CreateaccountDTO>> CreateAccount(CreateaccountDTO request)
        {
            var user = await _accountService.CreateAccountAsync(request);

            if(user is null)
            {
                return BadRequest("Please enter valid userid and account type");
            }

            return Ok(request);
        }
    }
}