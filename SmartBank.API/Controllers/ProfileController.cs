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
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _service;

        public ProfileController(IProfileService service)
        {
            this._service = service;
        }

        [HttpGet("get-profile")]

        public async Task<ActionResult<ProfileDTO>> GetProfile(string EmailId)
        {
            return Ok(await _service.GetProfileAsync(EmailId));
        }

        [HttpPut("edit-profile")]

        public async Task<ActionResult<EditProfileDTO>> EditProfile(EditProfileDTO profile)
        {
            return Ok(await _service.EditProfileAsync(profile));
        }

    }
}