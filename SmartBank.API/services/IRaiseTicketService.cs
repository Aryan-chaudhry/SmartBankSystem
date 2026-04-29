using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartBank.Data.DTO;

namespace SmartBank.API.services
{
    public interface IRaiseTicketService
    {
        Task<RaiseTicketDTO?> RaiseTicketAsync (RaiseTicketDTO ticket, string Email);

        Task<ViewTicketDTO?> ViewStatus(int TicketId); 

        Task<ResolveTicketDTO?> ResolveTicketAsync(ResolveTicketDTO ticket, int TicketId);
    }
}