using Lumina.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Data.Repository.Interfaces;

public  interface ITicketRepository : IGenericRepository<Ticket, Guid>
{
}
