using Lumina.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumina.Service.Common.DTO_s.Output.Screening;

public  record class ScreeningReadDto
{

    public Guid Id { get; init; }

    public Guid MovieId { get; init; } 

    public string MovieImageUrl { get; init; }

    public string MovieTitle { get; init; } 

    public Guid CinemaId { get; init; }

    public string CinemaName { get; init; }

    public int AvailableTickets { get; init; }   

    public string Showtime { get; init; } 

}
