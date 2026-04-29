using Microsoft.AspNetCore.Mvc;
using SmartBank.Data.DTO;

namespace SmartBank.API.services
{
    public interface IAccountService
    {
        Task<ActionResult<CreateaccountDTO>> CreateAccountAsync(CreateaccountDTO request);

        Task<ActionResult<decimal>> GetBalanceAsync(GetbalanceDTO request);

        Task<ActionResult<string?>> GetAccountStatusAsyc(GetaccountstatusDTO request);

    }
}