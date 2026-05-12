using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumina.Data.Models;

namespace Lumina.Service.Common.DTO_s.Input.Screening;

public   record class ScreeningCreateDto
{
    public Guid MovieId { get; init; } 

    public Guid CinemaId { get; init; } 

    public int AvailableTickets { get; init; }   

    public string Showtime { get; init; } = null!;
}
