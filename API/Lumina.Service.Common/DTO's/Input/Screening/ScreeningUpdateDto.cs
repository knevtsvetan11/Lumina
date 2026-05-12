using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Service.Common.DTO_s.Input.Screening;

public  record class ScreeningUpdateDto
{

    public Guid CinemaId { get; init; }

    public Guid MovieId { get; init; }

    public string Showtime { get; init; }

    public int AvailableTickets { get; init; }


}
