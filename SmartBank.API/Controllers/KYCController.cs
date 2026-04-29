using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartBank.API.services;
using SmartBank.Data.DTO;
using SmartBank.Data.Entities;

namespace SmartBank.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KYCController : ControllerBase
    {
        private readonly IKYCService _service;

        public KYCController(IKYCService service)
        {
            this._service = service;
        }

        [HttpPost]

        public async Task<ActionResult<UploadKYCDTO>> UploadKYC (UploadKYCDTO kyc, string Email)
        {
            var response = await _service.UploadKYCAsync(kyc, Email);

            if(response is null)
            {
                return BadRequest("Kyc Invalid");
            }

            return Ok(response);
        }
    }
}