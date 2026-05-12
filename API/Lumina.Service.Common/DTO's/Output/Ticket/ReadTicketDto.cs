using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Service.Common.DTO_s.Output.Ticket;

public  class ReadTicketDto
{
    public string CinemaName { get; set; }

    public string  ScreeningMovieName{ get; set; }

    public string Showtime { get; set; }

    public decimal  Price { get; set; }

    public int Count { get; set; }

}
