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
    public class LoanController : ControllerBase
    {
        private readonly IApplyLoanService _service;

        public LoanController(IApplyLoanService service)
        {
            this._service = service;
        }

        [HttpPost("apply-loan")]

        public async Task<ActionResult<LoanApplyDTO>> ApplyLoan(LoanApplyDTO loan, string Email)
        {
            var response = await _service.ApplyLoanAsyn(loan, Email);

            if(response is null)
            {
                return BadRequest("Invalid Details ");
            }

            return Ok(response);
        }

        [HttpPost("upload-loandocs")]

        public async Task<ActionResult<UploadLoanDocumentDTO>> UploadLoanDocs(UploadLoanDocumentDTO docs, string Email)
        {
            var response = await _service.UploadLoanDocsAsync(docs, Email);

            if(response is null)
            {
                return BadRequest("Invalid Docs");
            }

            return Ok(response);
        }

        // add authorise by admin
        [HttpPost("Loan-status")]
        public async Task<ActionResult<LoanStatusDTO>> LoanStatus(LoanStatusDTO loanApplied, string Email)
        {
            var response = await _service.LoanStatusAsync(loanApplied, Email);

            if(response is null)
            {
                return BadRequest("Invalid loan");
            }

            return Ok(response);
        }
    }
}