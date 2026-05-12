using Lumina.Data.Models;
using Lumina.Service.Common.DTO_s.Output.Ticket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Services.Core.Interfaces;

public interface ITicketService
{
    Task<Ticket> ExistTicket(Guid userId, Guid screeeningId);

    Task<bool> BuyAsync(Guid userId, Guid screeningId, int ticketsCount);

    Task<IEnumerable<ReadTicketDto>> GetAllTicketsAsync(Guid userId);
}
