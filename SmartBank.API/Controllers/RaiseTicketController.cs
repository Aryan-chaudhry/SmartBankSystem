using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SmartBank.API.services;
using SmartBank.Data.DTO;
using SmartBank.Data.Entities;

namespace SmartBank.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RaiseTicketController : ControllerBase
    {
        private readonly IRaiseTicketService _service;

        public RaiseTicketController(IRaiseTicketService service)
        {
            this._service = service;
        }

        [HttpPost("raise-ticket")]

        public async Task<ActionResult<RaiseTicketDTO>> RaiseTicket(RaiseTicketDTO ticket, string Email)
        {
            var response = await _service.RaiseTicketAsync(ticket, Email);

            if(response is null)
            {
                return BadRequest("Invalid Ticket");
            }

            return Ok(response);
        }

        [HttpGet("view-status")]
        public async Task<ActionResult<ViewTicketDTO>> ViewStatus(int TicketId)
        {
            var response = await _service.ViewStatus(TicketId);

            if(response is null)
            {
                return NotFound("No Ticket Found");
            }

            return Ok(response);
        }

        [HttpPost("resolve-Ticket")]

        public async Task<ActionResult<ResolveTicketDTO>> ResolveTicket(ResolveTicketDTO ticket, int TicketId)
        {
            var response = await _service.ResolveTicketAsync(ticket, TicketId);

            if(response is null)
            {
                return BadRequest("Invalid Ticket or TicketID");
            }

            return Ok(response);
        }
    }
}