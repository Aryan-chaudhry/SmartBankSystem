using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SmartBank.Data.Entities;
using SmartBank.Data.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

namespace SmartBank.API.services
{
    public class RaiseTicketService : IRaiseTicketService
    {
        private readonly SmartBankContext _context;

        public RaiseTicketService(SmartBankContext context)
        {
            this._context = context;
        }

        public async Task<RaiseTicketDTO?> RaiseTicketAsync (RaiseTicketDTO ticket, string Email)
        {
            if(ticket is null || Email is null)
            {
                return null;
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == Email);
            
            if(user is null)
            {
                return null;
            }

            var Ticket = new SupportTicket();

            Ticket.CreatedByUserId = user.UserId;
            Ticket.AssignedToUserId = ticket.AssignedToUserId;
            Ticket.Subject = ticket.Subject;
            Ticket.Description = ticket.Description;
            Ticket.Category = ticket.Category;
            Ticket.CreatedAt = ticket.CreatedAt;

            await _context.SupportTickets.AddAsync(Ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task<ViewTicketDTO?> ViewStatus(int TicketId)
        {
            
            var Ticket = await _context.SupportTickets.FindAsync(TicketId);

            if(Ticket is null)
            {
                return null;
            }

            var MyTicket = new ViewTicketDTO
            {
                AssignedToUserId = Ticket.AssignedToUserId,
                Subject = Ticket.Subject,
                Description = Ticket.Description,
                Category = Ticket.Category,
                Status = Ticket.Status,
                CreatedAt = Ticket.CreatedAt
            };

            return MyTicket;
        } 

        public async Task<ResolveTicketDTO?> ResolveTicketAsync(ResolveTicketDTO ticket, int TicketId)
        {
            var Ticket = await _context.SupportTickets.FindAsync(TicketId);

            if(Ticket is null)
            {
                return null;
            }

            Ticket.Priority = ticket.Priority;
            Ticket.Status = ticket.Status;
            Ticket.Resolution = ticket.Resolution;
            Ticket.UpdatedAt = ticket.UpdatedAt;
            Ticket.ResolvedAt = ticket.ResolvedAt;

            await _context.SaveChangesAsync();
            return ticket;

        }
    }
}