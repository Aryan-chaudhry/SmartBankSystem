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
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]

        public async Task<ActionResult<RegisteruserDTO>> register(RegisteruserDTO request)
        {
            var user = await _service.RegisterAsync(request);

            if(user is null)
            {
                return BadRequest("User already exist");
            }

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginuserDTO request)
        {
            var response = await _service.LoginAsync(request);

            if(response is null)
            {
                return BadRequest("Invalid Credentails");
            }

            return Ok(response);
        }
    }
}