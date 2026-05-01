using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartBank.API.services;

namespace SmartBank.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("get-daily-transactions")]
        public async Task<IActionResult> GetDailyTransaction()
        {
            var response = await _reportService.GetDailyTransactionReport();

            if(response is null)
            {
                return BadRequest("Cannot get daily transactions record");
            }

            return Ok(response);
        }

        [HttpGet("get-approved-loans")]
        public async Task<IActionResult> GetApprovedLoans()
        {
            var response = await _reportService.GetApprovedLoans();

            if(response is null)
            {
                return BadRequest("Cannot get the approved loans");
            }

            return Ok(response);
        }

        [HttpGet("get-active-users")]
        public async Task<IActionResult> GetActiveUsers()
        {
            var response = await _reportService.GetActiveUsers();

            if(response is null)
            {
                return BadRequest("Cannot get the active users");
            }

            return Ok(response);
        }
    }
}